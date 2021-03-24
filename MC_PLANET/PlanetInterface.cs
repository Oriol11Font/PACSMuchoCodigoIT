using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PACS_Objects;
using PACS_Utils;

namespace MC_PLANET
{
    public partial class PlanetInterface : Form
    {
        private readonly RsaKeysService _rsaKeysService = new RsaKeysService();
        private readonly DataAccessService _dataAccess = new DataAccessService();
        private readonly string _buttonOn = Application.StartupPath + "\\imgs\\buttonON.png";
        private readonly string _buttonOff = Application.StartupPath + "\\imgs\\buttonOFF.png";

        private delegate void SafeCallDelegate(string text);

        private TcpipSystemService _tcp;
        private TcpListener _listener;
        private Thread _listenerThread;
        private bool _active;
        private Planet _planet;
        private SpaceShip _spaceShip;

        private Dictionary<char, string> _encryptedLetters;

        public PlanetInterface()
        {
            InitializeComponent();
        }

        private void GenerateValidationCode(int idPlanet)
        {
            var validationCode = _rsaKeysService.GenerateKey(12);

            var sqlParams = new Dictionary<string, string>
            {
                {"validationcode", validationCode}, {"idplanet", idPlanet.ToString()}
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
                $@"[SYSTEM] Generated Validation Code and encrypted letters for planet {_planet.GetName()}");
        }

        private void GeneratePlanetKeys(int idPlanet)
        {
            PrintPanel(_rsaKeysService.GenerateRsaKeys(idPlanet));
        }

        private void PrintPanel(string message)
        {
            PlanetConsole.Items.Add(message);
        }

        private void ListenerServer()
        {
            while (_active)
            {
                var msg = _tcp.WaitingForResponse(_listener);
                if (msg != null)
                {
                    WriteTextSafe(msg);
                }
            }
        }

        private void HandleMessage(string msg)
        {
            try
            {
                var msgType = msg.Substring(0, 2).ToUpper();
                switch (msgType)
                {
                    case "ER":
                    {
                        PrintPanel($@"[SYSTEM] Rebut missatge de validació d'entrada: {msg}");

                        if (msg.Length == 26)
                        {
                            var shipCode = msg.Substring(2, 12);
                            var deliveryCode = msg.Substring(14, 12);

                            var sqlParams = new Dictionary<string, string>
                            {
                                {"codespaceship", shipCode}
                            };

                            var spaceshipRows =
                                _dataAccess.GetByQuery(
                                    "SELECT idSpaceShip, CodeSpaceShip, IPSpaceShip, PortSpaceShip FROM SpaceShips WHERE CodeSpaceShip = @codespaceship",
                                    sqlParams).Tables[0].Rows;
                            var spaceship = spaceshipRows[0].ItemArray;
                            _spaceShip = new SpaceShip(int.Parse(spaceship[0].ToString()), spaceship[1].ToString(),
                                spaceship[2].ToString(),
                                int.Parse(spaceship[3].ToString()));

                            sqlParams.Clear();

                            sqlParams.Add("codedelivery", deliveryCode);
                            var deliveryRows =
                                _dataAccess.GetByQuery("SELECT * FROM DeliveryData WHERE CodeDelivery = @codedelivery",
                                    sqlParams).Tables[0].Rows;

                            _tcp.SendMessageToServer(
                                $@"VR{_spaceShip.getCode()}{(spaceshipRows.Count == 1 && deliveryRows.Count == 1 ? "VP" : "AD")}",
                                _spaceShip.getIp(),
                                _spaceShip.getPort());
                        }
                        else
                        {
                            PrintPanel(@"[SYSTEM] El missatge de validació d'entrada no té 26 caràcters de longitud");
                        }

                        break;
                    }
                    case "VK":
                    {
                        PrintPanel($@"[SYSTEM] Rebut missatge de validació del codi de validació encriptat: {msg}");

                        if (msg.Length == 14)
                        {
                            var validationCode = msg.Substring(2);

                            var decryptedCode = _rsaKeysService.DecryptCode(validationCode, _planet.GetCode());

                            var validated = decryptedCode != null;

                            _tcp.SendMessageToServer(
                                $@"VR{_spaceShip.getCode()}{(validated ? "VP" : "AD")}",
                                _spaceShip.getIp(),
                                _spaceShip.getPort());

                            PrintPanel(
                                $@"[SYSTEM] Codi de validació rebut de la nau {_spaceShip.getCode()} {(validated ? "correctament" : "erròniament")} encriptat"
                            );

                            if (validated)
                            {
                                var zipPath = CreateZip();
                                PrintPanel(_tcp.SendFile(zipPath, _spaceShip.getIp(), _spaceShip.getPort()));
                            }
                        }
                        else
                        {
                            PrintPanel(
                                @"[SYSTEM] El missatge de validació del codi de validació no té 14 caràcters de longitud");
                        }

                        break;
                    }
                    default:
                    {
                        PrintPanel($@"[SYSTEM] S'ha rebut un missatge però en un format erroni: {msg}");
                        break;
                    }
                }
            }
            catch
            {
                PrintPanel(@"[SYSTEM] Error on handling entrance petition");
            }
        }

        private string CreateZip()
        {
            var filesPath = Path.Combine(Application.StartupPath, @"PacsFiles");
            var zipFilePath = Path.Combine(Application.StartupPath, @"zipfile.zip");

            if (Directory.Exists(filesPath)) Directory.Delete(filesPath, true);
            else Directory.CreateDirectory(filesPath);

            for (var i = 1; i <= 3; i++)
            {
                var decryptedFile = Path.Combine(filesPath, $@"DecryptedPACS{i}.txt");
                var encryptedFile = Path.Combine(filesPath, $@"PACS{i}.txt");

                CreateFile(decryptedFile, GenerateRandomLetters(100000), true);
                EncryptFile(decryptedFile, encryptedFile);

                // if (File.Exists(decryptedFile)) File.Delete(decryptedFile);
            }

            if (File.Exists(zipFilePath)) File.Delete(zipFilePath);
            ZipFile.CreateFromDirectory(filesPath, zipFilePath);

            return zipFilePath;
        }

        private void CreateFile(string path, string content, bool deleteIfExists)
        {
            if (File.Exists(path) && deleteIfExists) File.Delete(path);
            using (var tw = new StreamWriter(path, true))
            {
                tw.Write(content);
                tw.Flush();
            }
        }

        private void EncryptFile(string originFile, string encryptedPath)
        {
            var iStream = new FileStream(originFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(iStream);

            using (var sw = File.CreateText(encryptedPath))
            {
                var res = sr.Read();
                while (res != -1)
                {
                    sw.Write(_encryptedLetters[(char) res]);
                    res = sr.Read();
                }

                sw.Flush();
                sw.Close();
            }

            sr.Close();
            iStream.Close();
        }

        private string GenerateRandomLetters(int length)
        {
            var strBuild = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < length; i++)
            {
                var flt = random.NextDouble();
                var shift = Convert.ToInt32(Math.Floor(25 * flt));
                var letter = Convert.ToChar(shift + 65);
                strBuild.Append(letter);
            }

            return strBuild.ToString();
        }

        //LLEGIR CLAU ENVIADA PER LA NAU

        //DESENCRIPTAR LA CLAU
        private void PlanetInterface_Load(object sender, EventArgs e)
        {
            var res = _dataAccess.GetTable(@"Planets");
            planetCmbx.DataSource = res.Tables[0];
            planetCmbx.ValueMember = @"idPlanet";
            planetCmbx.DisplayMember = @"DescPlanet";
            onOffButton.ImageLocation = _buttonOff;
            planetCmbx_ValueMemberChanged(null, null);

            var idPlanet = int.Parse(planetCmbx.SelectedValue.ToString());

            GenerateValidationCode(idPlanet);
            GeneratePlanetKeys(idPlanet);
        }

        private void planetCmbx_ValueMemberChanged(object sender, EventArgs e)
        {
            var sqlParams = new Dictionary<string, string> {{"idplanet", planetCmbx.SelectedValue.ToString()}};
            var planetRow = _dataAccess.GetByQuery(
                @"SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet FROM Planets WHERE idPlanet = @idplanet",
                sqlParams).Tables[0].Rows[0].ItemArray;
            _planet = new Planet(int.Parse(planetRow[0].ToString()), planetRow[1].ToString(), planetRow[2].ToString(),
                planetRow[3].ToString(), int.Parse(planetRow[4].ToString()));

            PrintPanel($@"[SYSTEM] - Selected Planet {_planet.GetName()}");
        }

        private void OnOffButton_Click(object sender, EventArgs e)
        {
            if (!_active)
            {
                _active = true;
                onOffButton.ImageLocation = _buttonOn;

                _tcp = new TcpipSystemService();
                _listener = _tcp.StartServer(_planet.GetPort(), _listener);

                _listenerThread = new Thread(ListenerServer)
                {
                    IsBackground = true
                };
                _listenerThread.Start();

                PrintPanel(
                    $@"[SYSTEM] - Started server for planet {_planet.GetName()}. Listening on port {_planet.GetPort()}");
            }
            else
            {
                _active = false;
                _listenerThread.Abort();
                _listener = _tcp.StopServer(_listener);
                onOffButton.ImageLocation = _buttonOff;
                //_tcp = null;
                //_listener = null;
                PrintPanel(
                    @"[SYSTEM] - Stopped server");
            }
        }

        private void txtb_msg_TextChanged(object sender, EventArgs e)
        {
            if (txtb_msg.Text.Length > 2)
            {
                HandleMessage(txtb_msg.Text);
            }
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
    }
}