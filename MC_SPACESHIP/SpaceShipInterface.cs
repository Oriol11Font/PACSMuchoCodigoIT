using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using PACS_Objects;
using PACS_Utils;

namespace MC_SPACESHIP
{
    public partial class SpaceShipInterface : Form
    {
        private readonly string _buttonOff = Path.Combine(Application.StartupPath, "imgs", "buttonOFF.png");

        private readonly string _buttonOn = Path.Combine(Application.StartupPath, "imgs", "buttonON.png");

        private readonly DataAccessService _dt = new DataAccessService();
        private readonly RsaKeysService _rsa = new RsaKeysService();
        private readonly TcpipSystemService _tcp = new TcpipSystemService();

        private readonly bool[] _validations = {false, false, false};

        private delegate void SafeCallDelegate(string text);

        private Point _lastLocation;
        private bool _mouseDown;
        private bool _active;
        private TcpListener _listener, _fileListener;
        private Planet _planet;
        private SpaceShip _spaceShip;

        private Thread _t1, _t2;

        public SpaceShipInterface()
        {
            InitializeComponent();
        }


        private void SpaceShipInterface_Load(object sender, EventArgs e)
        {
            onOffButton.ImageLocation = _buttonOff;
            //COMBOBOX AMB ELS PLANETES A ESCOLLIR
            const string sqlSpaceShip =
                "SELECT idSpaceShip, CodeSpaceShip, IPSpaceShip, PortSpaceShip1, PortSpaceShip2 FROM SpaceShips WHERE CodeSpaceShip = 'X-Wing R0001';";

            var dr = _dt.GetByQuery(sqlSpaceShip).Tables[0].Rows[0].ItemArray;

            _spaceShip = new SpaceShip(int.Parse(dr[0].ToString()),
                dr[1].ToString(), dr[2].ToString(),
                int.Parse(dr[3].ToString()), int.Parse(dr[4].ToString()));

            const string sql = "SELECT DescPlanet FROM Planets;";
            var ds2 = _dt.GetByQuery(sql);

            var planets = new List<object>();

            foreach (DataRow row in ds2.Tables[0].Rows) comboPlanet.Items.Add(row["DescPlanet"]);
        } //AL INICIAR EL FORMULARI

        private void ping_Click(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem == null) return;
            if (TcpipSystemService.CheckXarxa("8.8.8.8", 5))
            {
                PrintPanel("[SYSTEM] - Stable connection with" + _planet.GetName());
                TcpipSystemService.SendMessageToServer("ER" + _spaceShip.GetCode() + "DELIVER01TAK", _planet.GetIp(),
                    _planet.GetPort());
            }
            else
            {
                PrintPanel("[ERROR] - Failed connection to " + _planet.GetName() + " : " + _planet.GetIp());
            }
        } //COMPROVACIO PING AL PLANETA

        private void comboPlanet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem == null) return;
            btn_detectPlanet.Enabled = true;
            //btn_SendCode.Enabled = true;

            var sql =
                "SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet, PortPlanet1 FROM Planets WHERE DescPlanet = '" +
                comboPlanet.SelectedItem + "';";
            var ds = _dt.GetByQuery(sql);

            var dr = ds.Tables[0].Rows[0];

            _planet = new Planet(int.Parse(dr.ItemArray.GetValue(0).ToString()),
                dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(2).ToString(),
                dr.ItemArray.GetValue(3).ToString(), int.Parse(dr.ItemArray.GetValue(4).ToString()), int.Parse(dr.ItemArray.GetValue(5).ToString()));

            var mssg = "ER" + _spaceShip.GetCode() + "DADAD";

            try
            {
                PrintPanel("[SYSTEM] - Selected Planet: " + _planet.GetCode() + " | " + _planet.GetName() +
                           " - Address: " + _planet.GetIp() + " - Port: " + _planet.GetPort() +
                           " - Ready to CHECK");

                //tcp.SendMessageToServer(mssg, planet.getIp(), planet.getPort());
            }
            catch
            {
                PrintPanel("[ERROR] - Error connecting to Server of the Planet");
            }
        } //SELECIO DEL PLANETA QUE ES VOL ACCEDIR

        private void PrintPanel(string message)
        {
            if (message == "") return;
            SpaceShipConsole.Text = SpaceShipConsole.Text + message + Environment.NewLine;

            if (SpaceShipConsole.Items.Count > 30) SpaceShipConsole.Items.Clear();

            SpaceShipConsole.Items.Add(message);
        } //PER FER PRINT A LA CONSOLA DE LA PANTALLA

        private void StartServer_Click(object sender, EventArgs e)
        {
            if (_active)
                try
                {
                    btn_detectPlanet.Enabled = false;
                    btn_SendCode.Enabled = false;
                    comboPlanet.Enabled = false;
                    SpaceShipConsole.Items.Clear();

                    _active = false;
                    infoSpaceShip.Items.Clear();
                    onOffButton.ImageLocation = _buttonOff;
                    _t1.Abort();
                    _listener = TcpipSystemService.StopServer(_listener);
                    PrintPanel("[SYSTEM] - Server OFF");
                }
                catch
                {
                    PrintPanel("[ERROR] - Failed to stop the Server Process");
                }
            else
                try
                {
                    _active = true;
                    comboPlanet.Enabled = true;
                    infoSpaceShip.Items.Add("SpaceShip:");
                    infoSpaceShip.Items.Add("Code: " + _spaceShip.GetCode());
                    infoSpaceShip.Items.Add("IP: " + _spaceShip.GetIp());
                    infoSpaceShip.Items.Add("Port: " + _spaceShip.GetPort1());
                    onOffButton.ImageLocation = _buttonOn;
                    _listener = TcpipSystemService.StartServer(_spaceShip.GetPort1(), _listener);
                    _t1 = new Thread(ListenerServer) {IsBackground = true};
                    _t1.Start();

                    _fileListener = TcpipSystemService.StartServer(_spaceShip.GetPort2(), _fileListener);
                    _t2 = new Thread(RecivedFiles);
                    _t2.IsBackground = true;
                    _t2.Start();

                    PrintPanel("[SYSTEM] - Server ON");
                }
                catch
                {
                    PrintPanel("[ERROR] - Failed to start the server");
                }
        } //INICIAR SERVIDOR

        private void ListenerServer()
        {
            while (_active)
            {
                var mssg = _tcp.WaitingForResponse(_listener, _planet);
                if (mssg != null) WriteTextSafe(mssg);
            }
        } //BUSTIA DE MISSATGES PER PART DEL SERVIDOR

        private void RecivedMessage(string message)
        {
            try
            {
                var idMessage = message.Substring(0, 2);
                var result = message.Substring(14, 2);
                switch (idMessage)
                {
                    case "VR":
                        switch (result)
                        {
                            case "VP":
                            if (_validations[0]){
                                _validations[1] = true;
                                
                            }
                            else
                            {
                                _validations[0] = true;
                            }

                                PrintPanel("[SYSTEM] - Validation in progress...");
                                break;
                            case "AD":
                            {
                                if (_validations[0])
                                    _validations[1] = false;
                                else
                                    _validations[0] = false;

                                PrintPanel("[ERROR] - ACCESS DENIED, please leave the security area");
                                break;
                            }
                            case "AG":
                                _validations[2] = true;
                                PrintPanel("[SYSTEM] - Validation successfully, enjoy your visit!");
                                break;
                        }

                        break;
                }
            }
            catch
            {
                PrintPanel(messageRecived.Text.Length.ToString());
                //check = false;
            }
        } //DESCODIFICADOR DE MISSATGES DE VERIFICACIO

        private void messageRecived_TextChanged(object sender, EventArgs e)
        {
            if (messageRecived.Text.Length <= 2) return;
            if (messageRecived.Text == @"code")
            {
                var queryCode = $@"SELECT ValidationCode FROM InnerEncryption WHERE idPlanet = {_planet.GetId()}";
                var code = _dt.GetByQuery(queryCode).Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                var encryptedCode = _rsa.EncryptedCode(code, _planet.GetId().ToString());

                TcpipSystemService.SendMessageToServer(encryptedCode, _planet.GetIp(), _planet.GetPort());
            }

            RecivedMessage(messageRecived.Text);
            ValidationNextStep();
        } //DETECTA PER INICIAR LA DESCODIFICACIO

        private void ValidationNextStep()
        {
            if (_validations[0])
            {
                btn_SendCode.Enabled = true;
                btn_detectPlanet.ForeColor = Color.Green;
                btn_detectPlanet.FlatAppearance.BorderColor = Color.Green;
                if (!_validations[1]) return;
                btn_SendFiles.Enabled = true;
                btn_SendCode.ForeColor = Color.Green;
                btn_detectPlanet.FlatAppearance.BorderColor = Color.Green;

                if (!_validations[2]) return;
                btn_SendFiles.ForeColor = Color.Green;
                btn_detectPlanet.FlatAppearance.BorderColor = Color.Green;
            }
            else
            {
                //validations = new bool[] { false, false, false };
                btn_detectPlanet.ForeColor = Color.Red;
                btn_detectPlanet.FlatAppearance.BorderColor = Color.Red;
            }
        } //ACTIVANT BOTONS QUAN VAN PASSANT PROCESSOS DE VALIDACIÓ

        private void GetCodeAndSend()
        {
            try
            {
                var sqlParams = new Dictionary<string, dynamic> {{@"planetid", _planet.GetId()}};
                var code = _dt.GetByQuery("SELECT ValidationCode FROM InnerEncryption WHERE idPlanet = @planetid",
                        sqlParams)
                    .Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                var encryptedCode = _rsa.EncryptedCode(code, _planet.GetId().ToString());

                TcpipSystemService.SendMessageToServer("VK", _planet.GetIp(), _planet.GetPort());

                PrintPanel("[SYSTEM] - Key validation send it, waiting for response...");
            }
            catch
            {
                PrintPanel("[ERROR] - Key validation error, problem with connecting to server");
            }
        } //DEMANAR LA CLAU I EL CODI A LA BBDD I ENVIARLO

        private void offButton_Click(object sender, EventArgs e)
        {
            Close();
        } //TANCA LA PAGINA DE LA NAU

        private void SendCodeButton_Click(object sender, EventArgs e)
        {
            GetCodeAndSend();
        } //BOTON PARA ENVIAR EL CODIGO ENCRUPTADO AL PLANETA

        private void WriteTextSafe(string text)
        {
            if (messageRecived.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                messageRecived.Invoke(d, text);
            }
            else
            {
                messageRecived.Text = "";
                messageRecived.Text = text;
            }
        } //FUNCIO PER PODER ESCRIURE AL TEXT BOX SENSE QUE PETI EL THREAD

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

        private void DecodeFiles()
        {
            var extractPath = Application.StartupPath + "\\ZipFiles";
            var downloadPath = Application.StartupPath + "\\Downloads";

            var strFiles = Directory.GetFiles(extractPath, "*", SearchOption.AllDirectories).ToList();

            foreach (var fichero in strFiles) File.Delete(fichero);

            ZipFile.ExtractToDirectory(downloadPath, extractPath);

            var res = _dt.GetByQuery(
                "SELECT Word, Numbers FROM InnerEncryptionData as IED LEFT JOIN InnerEncryption as IE on IE.idInnerEncryption = IED.IdInnerEncryption WHERE IE.idPlanet = " +
                _planet.GetId() + ";");
            var abcCodes = new Dictionary<string, string>();

            for (var i = 0; i < 24; i++)
            {
                var dr = res.Tables[0].Rows[i];
                abcCodes.Add(dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(0).ToString());
            }

            var pathFile = Application.StartupPath + "\\ZipFiles\\PacsSol.txt";

            var iStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(iStream);

            foreach (var sw in strFiles
                .Select(file => new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Read))
                .Select(oStream => new StreamWriter(oStream)))
            {
                string code;
                string letter;

                while (sr.Peek() >= 0)
                {
                    code = sr.ReadLine();

                    letter = abcCodes[code ?? string.Empty];

                    sw.WriteLine(letter);
                    sw.Flush();
                }

                sw.Close();
            }

            sr.Close();
        } //FUNCIO A EXECUTAR QUAN LA NAU REP ELS TRES ARXIUS

        private void RecivedFiles()
        {
            while (_active)
            {
                String mssg = TcpipSystemService.RecivedFile(_fileListener, Application.StartupPath + "\\Downloads\\");
                if (mssg != null)
                {
                    WriteTextSafe(mssg);
                }
            }
        }

        private void btn_SendFiles_Click(object sender, EventArgs e)
        {
            string fileToUnzip = Application.StartupPath + "\\Downloads\\PACS.zip";
            string fileToSend = Application.StartupPath + "\\Downloads\\PACSSSOL.zip";
            string fileToJoinTxt = Application.StartupPath + "\\Downloads\\PACS.txt";
            string fileDecrypted = Application.StartupPath + "\\Downloads\\PACSSol.txt";
            string extractDirectory = Application.StartupPath + "\\Downloads\\extractFiles";

            FileManagement.UnzipFile(fileToUnzip, extractDirectory);

            IEnumerable<string> filePaths = Directory.GetFiles(extractDirectory, "*", SearchOption.AllDirectories).ToList();

            FileManagement.JoinTxtFiles(filePaths, fileToJoinTxt);

            var res = _dt.GetByQuery("SELECT Word, Numbers FROM InnerEncryptionData as IED LEFT JOIN InnerEncryption as IE on IE.idInnerEncryption = IED.IdInnerEncryption WHERE IE.idPlanet = " + _planet.GetId() + ";");
            Dictionary<char, string> abcCodes = new Dictionary<char, string>();

            for (int i = 0; i < 24; i++)
            {
                var dr = res.Tables[0].Rows[i];
                abcCodes.Add(dr.ItemArray.GetValue(0).ToString()[0], dr.ItemArray.GetValue(1).ToString());
            }

            FileManagement.DecryptFile(fileToJoinTxt, fileDecrypted, abcCodes);

            List<string> filesToZip = new List<string> { fileDecrypted };

            FileManagement.ZipFile(fileToSend, filesToZip);

            //tcp.SendFile(fileToSend, planet.GetIp());

        }
    }
}