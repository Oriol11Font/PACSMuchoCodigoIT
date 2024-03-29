﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using PACS_Objects;
using PACS_Utils;

namespace MC_PLANET
{
    public partial class PlanetInterface : Form
    {
        private readonly string _buttonOff = $@"{Application.StartupPath}\\imgs\\buttonOFF.png";
        private readonly string _buttonOn = $@"{Application.StartupPath}\\imgs\\buttonON.png";
        private readonly DataAccessService _dataAccess = new DataAccessService();
        private readonly RsaKeysService _rsaKeysService = new RsaKeysService();
        private delegate void SafeCallDelegate(string text);
        private bool _active;

        private Dictionary<char, string> _encryptedLetters;
        private TcpListener _listener, _fileListener;
        private Thread _t1, _t2;
        private Planet _planet;
        private SpaceShip _spaceShip;
        private Point _lastLocation;
        private bool _mouseDown;
        private TcpipSystemService _tcp;
        private bool _code;

        public PlanetInterface()
        {
            InitializeComponent();
        }

        private void GenerateValidationCode(int idPlanet)
        {
            var validationCode = _rsaKeysService.GenerateKey(12);

            var sqlParams = new Dictionary<string, dynamic>
            {
                {"validationcode", validationCode}, {"idplanet", idPlanet}
            };
            _dataAccess.RunSafeQuery(@"
                BEGIN TRANSACTION
                UPDATE dbo.InnerEncryption SET ValidationCode = @validationcode WHERE idPlanet = @idplanet;
                IF @@ROWCOUNT = 0
                BEGIN
                    INSERT dbo.InnerEncryption(idPlanet, ValidationCode) VALUES (@idplanet, @validationcode);
                END;
                COMMIT TRANSACTION;
            ", sqlParams);

            var idinnerencryption = int.Parse(_dataAccess.GetByQuery(
                    $@"SELECT idInnerEncryption FROM InnerEncryption WHERE idPlanet = {planetCmbx.SelectedValue};")
                .Tables[0].Rows[0].ItemArray[0].ToString());

            _encryptedLetters = _rsaKeysService.GenerateEncryptedChars();

            sqlParams.Clear();
            sqlParams.Add("idinnerencryption", idinnerencryption.ToString());

            var values = _encryptedLetters
                .Select(letterPair => $@"({idinnerencryption}, '{letterPair.Key}', '{letterPair.Value}')").ToList();

            _dataAccess.RunSafeQuery(
                $@"BEGIN TRANSACTION; DELETE FROM InnerEncryptionData WHERE idInnerEncryption = @idinnerencryption; INSERT INTO InnerEncryptionData (IdInnerEncryption, Word, Numbers) VALUES {string.Join(", ", values)}; COMMIT TRANSACTION;",
                sqlParams);

            PrintPanel(
                $@"[SYSTEM] - Generated Validation Code and encrypted letters for planet {_planet.GetName()}");
        }

        private void GeneratePlanetKeys(string codePlanet, int idPlanet)
        {
            PrintPanel(_rsaKeysService.GenerateRsaKeys(codePlanet, idPlanet));
        }

        private void PrintPanel(string message)
        {
            PlanetConsole.Items.Add(message);
        }

        private void ListenerServer()
        {
            while (_active)
            {
                var msg = _tcp.WaitingForResponse(_listener, _planet);
                if (msg != null) WriteTextSafe(msg);
            }
        }

        private void HandleMessage(string msg)
        {
            try
            {
                var msgType = msg.Substring(0, 2).ToUpper();
                if (_code) msgType = "VK";
                if (msgType == "VK") _code = !_code;

                switch (msgType)
                {
                    case "ER":
                    {
                        PrintPanel($@"[SYSTEM] - Rebut missatge de validació d'entrada: {msg}");

                        if (msg.Length == 26)
                        {
                            var shipCode = msg.Substring(2, 12);
                            var deliveryCode = msg.Substring(14, 12);

                            var sqlParams = new Dictionary<string, dynamic>
                            {
                                {"codespaceship", shipCode}
                            };

                            var spaceshipRows =
                                _dataAccess.GetByQuery(
                                    "SELECT idSpaceShip, CodeSpaceShip, IPSpaceShip, PortSpaceShip1, PortSpaceShip2 FROM SpaceShips WHERE CodeSpaceShip = @codespaceship",
                                    sqlParams).Tables[0].Rows;
                            var spaceship = spaceshipRows[0].ItemArray;
                            _spaceShip = new SpaceShip(int.Parse(spaceship[0].ToString()), spaceship[1].ToString(),
                                spaceship[2].ToString(),
                                int.Parse(spaceship[3].ToString()), int.Parse(spaceship[4].ToString()));

                            sqlParams.Clear();

                            sqlParams.Add("codedelivery", deliveryCode);
                            var deliveryRows =
                                _dataAccess.GetByQuery("SELECT * FROM DeliveryData WHERE CodeDelivery = @codedelivery",
                                    sqlParams).Tables[0].Rows;

                            TcpipSystemService.SendMessageToServer(
                                $@"VR{_spaceShip.GetCode()}{(spaceshipRows.Count == 1 && deliveryRows.Count == 1 ? "VP" : "AD")}",
                                _spaceShip.GetIp(),
                                _spaceShip.GetPort1());
                        }
                        else
                        {
                            PrintPanel(@"[SYSTEM] - El missatge de validació d'entrada no té 26 caràcters de longitud");
                        }

                        break;
                    }
                    case "VK":
                    {
                        if (msg.Length > 2)
                        {
                            PrintPanel(@"[SYSTEM] - Rebut missatge de validació del codi de validació encriptat");

                            var decryptedCode = msg;

                            PrintPanel($@"[SYSTEM] - Verificant codi de validació de {_planet.GetName()}...");

                            var sqlParams = new Dictionary<string, dynamic> {{"validationcode", decryptedCode}};
                            var validationCodeRows = _dataAccess.GetByQuery(
                                "SELECT * FROM dbo.InnerEncryption WHERE ValidationCode = @validationcode", sqlParams);

                              // var validated = true;
                             var validated = validationCodeRows.Tables[0].Rows.Count >= 1;

                            TcpipSystemService.SendMessageToServer(
                                $@"VR{_spaceShip.GetCode()}{(validated ? "VP" : "AD")}",
                                _spaceShip.GetIp(),
                                _spaceShip.GetPort1());

                            PrintPanel(
                                $@"[SYSTEM] - Codi de validació rebut de la nau {_spaceShip.GetCode()} {(validated ? "correctament" : "erròniament")} encriptat"
                            );

                            if (validated)
                            {
                                var zipPath = CreateZip();
                                PrintPanel(_tcp.SendFile(zipPath, _spaceShip.GetIp(), _spaceShip.GetPort2()));
                            }
                        }
                        else
                        {
                            TcpipSystemService.SendMessageToServer("code", _spaceShip.GetIp(), _spaceShip.GetPort1());
                        }

                        break;
                    }
                    case "FS":
                    {
                        PrintPanel("[SYSTEM] - Files recived, starting validation ...");
                        string pathFiles = Application.StartupPath + "\\PacsFiles\\decrypted\\";
                        string[] files = { pathFiles + "\\PACS1.txt", pathFiles + "\\PACS2.txt", pathFiles + "\\PACS3.txt" };
                        string downloadsFld = Application.StartupPath + "\\Downloads";

                        FileManagement.SumFiles(files, downloadsFld + "\\PACSTotal.txt");

                        FileManagement.UnzipFile(downloadsFld + "\\PACS.zip", downloadsFld + "\\extractFiles");

                        string file1 = downloadsFld + "\\PACSTotal.txt";
                        string file2 = downloadsFld + "\\extractFiles\\PACSSOL.txt";

                        byte [] hash1 = FileManagement.CreateMD5(file1);
                        byte [] hash2 = FileManagement.CreateMD5(file2);

                        string mssg = "VR"+_spaceShip.GetCode()+"AD";
                        string infomsg = "[SYSTEM] - Incorrect encode, " + _spaceShip.GetCode() + " have to leave the area!";
                            if (hash1.SequenceEqual(hash2)) { 
                            mssg = "VR" + _spaceShip.GetCode() + "AG";
                            infomsg = "[SYSTEM] - Correct encode of files, accepted permission to acces in " + _planet.GetName();
                        }

                        TcpipSystemService.SendMessageToServer(mssg, _spaceShip.GetIp(), _spaceShip.GetPort1());
                        PrintPanel(infomsg);
                        break;
                    }
                    default:
                    {
                        PrintPanel($@"[SYSTEM] - S'ha rebut un missatge però en un format erroni: {msg}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                PrintPanel(ex.ToString());
            }
        }

        private string CreateZip()
        {
            var filesPath = Path.Combine(Application.StartupPath, @"PacsFiles");
            var zipFilePath = Path.Combine(filesPath, @"zipfile.zip");
            var encryptedDirectory = Path.Combine(filesPath, @"encrypted");
            var decryptedDirectory = Path.Combine(filesPath, @"decrypted");

            FileManagement.CreateDirectory(filesPath);
            FileManagement.CreateDirectory(decryptedDirectory);
            FileManagement.CreateDirectory(encryptedDirectory);

            for (var i = 1; i <= 3; i++)
            {
                var decryptedFile = Path.Combine(filesPath, decryptedDirectory, $@"PACS{i}.txt");
                FileManagement.CreateFile(decryptedFile, RsaKeysService.GenerateRandomLetters(100000));

                var encryptedFile = Path.Combine(encryptedDirectory, $@"PACS{i}.txt");
                FileManagement.EncryptFile(decryptedFile, encryptedFile, _encryptedLetters);
            }

            FileManagement.ZipFile(encryptedDirectory, zipFilePath);
            return zipFilePath;
        }

        private void PlanetInterface_Load(object sender, EventArgs e)
        {
            var res = _dataAccess.GetTable(@"Planets");
            planetCmbx.DataSource = res.Tables[0];
            planetCmbx.ValueMember = @"idPlanet";
            planetCmbx.DisplayMember = @"DescPlanet";
            onOffButton.ImageLocation = _buttonOff;
            planetCmbx_ValueMemberChanged(null, null);

            var idPlanet = int.Parse(planetCmbx.SelectedValue.ToString());
            var sql = $@"SELECT CodePlanet FROM Planets Where idPlanet = {idPlanet}";
            var codePlanet = _dataAccess.GetByQuery(sql).Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

            GenerateValidationCode(idPlanet);
            GeneratePlanetKeys(codePlanet, idPlanet);
        }

        private void planetCmbx_ValueMemberChanged(object sender, EventArgs e)
        {
            var sqlParams = new Dictionary<string, dynamic> {{"idplanet", planetCmbx.SelectedValue.ToString()}};
            var planetRow = _dataAccess.GetByQuery(
                @"SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet, PortPlanet1 FROM Planets WHERE idPlanet = @idplanet",
                sqlParams).Tables[0].Rows[0].ItemArray;
            _planet = new Planet(int.Parse(planetRow[0].ToString()), planetRow[1].ToString(),
                planetRow[2].ToString(),
                planetRow[3].ToString(), int.Parse(planetRow[4].ToString()), int.Parse(planetRow[5].ToString()));

            PrintPanel($@"[SYSTEM] - Selected Planet {_planet.GetName()}");
        }

        private void OnOffButton_Click(object sender, EventArgs e)
        {
            if (!_active)
            {
                _active = true;
                onOffButton.ImageLocation = _buttonOn;

                _tcp = new TcpipSystemService();
                _listener = TcpipSystemService.StartServer(_planet.GetPort(), _listener);

                _t1 = new Thread(ListenerServer)
                {
                    IsBackground = true
                };
                _t1.Start();

                
                _fileListener = TcpipSystemService.StartServer(_planet.GetPort1(), _fileListener);
                _t2 = new Thread(RecivedFiles);
                _t2.IsBackground = true;
                _t2.Start();

                PrintPanel(
                    $@"[SYSTEM] - Started server for planet {_planet.GetName()}. Listening on port {_planet.GetPort()}");
            }
            else
            {
                _active = false;
                if (_t1 != null)
                {
                    _t1.Abort();
                }
                if (_t2 != null)
                {
                    _t2.Abort();
                }
                _listener = TcpipSystemService.StopServer(_listener);
                _fileListener = TcpipSystemService.StopServer(_fileListener);

                onOffButton.ImageLocation = _buttonOff;
                PrintPanel(
                    @"[SYSTEM] - Stopped server");
            }
        }

        private void txtb_msg_TextChanged(object sender, EventArgs e)
        {
            if (txtb_msg.Text.Length >= 2) HandleMessage(txtb_msg.Text);
        }

        private void WriteTextSafe(string text)
        {
            if (txtb_msg.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                txtb_msg.Invoke(d, text);
            }
            else
            {
                txtb_msg.Text = "";
                txtb_msg.Text = text;
            }
        }

        private void RecivedFiles()
        {
            while (_active)
            {
                String mssg = TcpipSystemService.RecivedFile(_fileListener, Application.StartupPath + "\\Downloads\\");
                if (mssg != null)
                {
                    WriteTextSafe("FS");
                }
            }
        }

        private void OffButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        //BARRA MOVIMENT DE LA PANTALLA
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _lastLocation = e.Location;
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown) return;
            //ChangeBorderColor(Color.Red);
            Location = new Point(
                Location.X - _lastLocation.X + e.X, Location.Y - _lastLocation.Y + e.Y);

            Update();
        }

        private void panel4_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            //ChangeBorderColor(Color.Yellow);
        }
    }
}