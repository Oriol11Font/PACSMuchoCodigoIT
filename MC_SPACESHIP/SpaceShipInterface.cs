using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PACS_Objects;
using PACS_Utils;
using System.Threading;
using System.Net.Sockets;
using System.IO.Compression;
using System.IO;
using System.Linq;

namespace MC_SPACESHIP
{
    public partial class SpaceShipInterface : Form
    {
        public SpaceShipInterface()
        {
            InitializeComponent();
        }

        readonly string buttonON = Application.StartupPath + "\\imgs\\buttonON.png";
        readonly string buttonOFF = Application.StartupPath + "\\imgs\\buttonOFF.png";

        readonly DataAccessService dt = new DataAccessService();
        readonly TcpipSystemService tcp = new TcpipSystemService();
        readonly RsaKeysService rsa = new RsaKeysService();


        private delegate void SafeCallDelegate(string text);

        private Point _lastLocation;
        private bool _mouseDown;
        //bool check = false;
        bool[] validations = {false, false, false};

        Thread t1, t2;
        TcpListener Listener, FileListener;
        Planet planet;
        SpaceShip spaceShip;
        bool active;


        private void SpaceShipInterface_Load(object sender, EventArgs e)
        {
            onOffButton.ImageLocation = buttonOFF;
            //COMBOBOX AMB ELS PLANETES A ESCOLLIR
            string sqlSpaceShip =
                "SELECT idSpaceShip, CodeSpaceShip, IPSpaceShip, PortSpaceShip1, PortSpaceShip2 FROM SpaceShips WHERE CodeSpaceShip = 'X-Wing R0001';";

            var dr = dt.GetByQuery(sqlSpaceShip).Tables[0].Rows[0].ItemArray;

            spaceShip = new SpaceShip(Int32.Parse(dr[0].ToString()),
                dr[1].ToString(), dr[2].ToString(),
                Int32.Parse(dr[3].ToString()), int.Parse(dr[4].ToString()));

            string sql = "SELECT DescPlanet FROM Planets;";
            DataSet ds2 = dt.GetByQuery(sql);

            var planets = new List<object>();

            foreach (DataRow row in ds2.Tables[0].Rows)
            {
                comboPlanet.Items.Add(row["DescPlanet"]);
            }
        } //AL INICIAR EL FORMULARI

        private void ping_Click(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null)
            {
                try
                {
                    if (tcp.CheckXarxa("8.8.8.8", 5))
                    {
                        printPanel("[SYSTEM] - Stable connection with" + planet.GetName());
                        tcp.SendMessageToServer("ER" + spaceShip.GetCode() + "DELIVER01TAK", planet.GetIp(),
                            planet.GetPort());
                    }
                    else
                    {
                        printPanel("[ERROR] - Failed connection to " + planet.GetName() + " : " + planet.GetIp());
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        } //COMPROVACIO PING AL PLANETA

        private void comboPlanet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null)
            {
                btn_detectPlanet.Enabled = true;
                //btn_SendCode.Enabled = true;

                var sql =
                    "SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet FROM Planets WHERE DescPlanet = '" +
                    comboPlanet.SelectedItem + "';";
                var ds = dt.GetByQuery(sql);

                var dr = ds.Tables[0].Rows[0];

                planet = new Planet(Int32.Parse(dr.ItemArray.GetValue(0).ToString()),
                    dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(2).ToString(),
                    dr.ItemArray.GetValue(3).ToString(), Int32.Parse(dr.ItemArray.GetValue(4).ToString()));

                string mssg = "ER" + spaceShip.GetCode() + "DADAD";

                try
                {
                    printPanel("[SYSTEM] - Selected Planet: " + planet.GetCode() + " | " + planet.GetName() +
                               " - Address: " + planet.GetIp() + " - Port: " + planet.GetPort() + " - Ready to CHECK");

                    //tcp.SendMessageToServer(mssg, planet.getIp(), planet.getPort());
                }
                catch (Exception ex)
                {
                    printPanel("[ERROR] - Error connecting to Server of the Planet");
                }
            }
        } //SELECIO DEL PLANETA QUE ES VOL ACCEDIR

        private void printPanel(string message)
        {
            if (message != "")
            {
                SpaceShipConsole.Text = SpaceShipConsole.Text + message + Environment.NewLine;

                if (SpaceShipConsole.Items.Count > 30)
                {
                    SpaceShipConsole.Items.Clear();
                }

                SpaceShipConsole.Items.Add(message);
            }
        } //PER FER PRINT A LA CONSOLA DE LA PANTALLA

        private void StartServer_Click(object sender, EventArgs e)
        {
            if (active == true)
            {
                try
                {
                    btn_detectPlanet.Enabled = false;
                    btn_SendCode.Enabled = false;
                    comboPlanet.Enabled = false;
                    SpaceShipConsole.Items.Clear();

                    active = false;
                    infoSpaceShip.Items.Clear();
                    onOffButton.ImageLocation = buttonOFF;
                    t1.Abort();
                    Listener = tcp.StopServer(Listener);
                    printPanel("[SYSTEM] - Server OFF");
                }
                catch
                {
                    printPanel("[ERROR] - Failed to stop the Server Process");
                }
            }
            else
            {
                try
                {
                    active = true;
                    comboPlanet.Enabled = true;
                    infoSpaceShip.Items.Add("SpaceShip:");
                    infoSpaceShip.Items.Add("Code: " + spaceShip.GetCode());
                    infoSpaceShip.Items.Add("IP: " + spaceShip.GetIp());
                    infoSpaceShip.Items.Add("Port: " + spaceShip.GetPort1());
                    onOffButton.ImageLocation = buttonON;
                    Listener = tcp.StartServer(spaceShip.GetPort1(), Listener);
                    t1 = new Thread(ListenerServer);
                    t1.IsBackground = true;
                    t1.Start();

                    FileListener = tcp.StartServer(spaceShip.GetPort2, FileListener);
                    t2 = new Thread(RecivedFiles);
                    t2.IsBackground = true;
                    t2.Start();

                    printPanel("[SYSTEM] - Server ON");
                }
                catch
                {
                    printPanel("[ERROR] - Failed to start the server");
                }
            }
        } //INICIAR SERVIDOR

        private void ListenerServer()
        {
            string mssg;

            while (active)
            {
                mssg = tcp.WaitingForResponse(Listener, planet);
                if (mssg != null) { WriteTextSafe(mssg); }

            }
        } //BUSTIA DE MISSATGES PER PART DEL SERVIDOR

        private void RecivedMessage(string message)
        {
            try
            {      
                string id_message = message.Substring(0, 2);
                string result = message.Substring(14, 2);
                switch (id_message)
                {
                    case "VR":
                        if (result.Equals("VP"))
                        {
                            if (validations[0])
                            {
                                validations[1] = true;
                                
                            }
                            else
                            {
                                validations[0] = true;
                            }

                            printPanel("[SYSTEM] - Validation in progress...");
                        }
                        else if (result.Equals("AD"))
                        {
                            if (validations[0])
                            {
                                validations[1] = false;
                            }
                            else
                            {
                                validations[0] = false;
                            }

                            printPanel("[ERROR] - ACCESS DENIED, please leave the security area");
                        }
                        else if (result.Equals("AG"))
                        {
                            validations[2] = true;
                            printPanel("[SYSTEM] - Validation successfully, enjoy your visit!");
                        }

                        break;
                }
            }
            catch
            {
                printPanel(messageRecived.Text.Length.ToString());
                //check = false;
            }
        } //DESCODIFICADOR DE MISSATGES DE VERIFICACIO

        private void messageRecived_TextChanged(object sender, EventArgs e)
        {
            if (messageRecived.Text.Length > 2)
            {
                if (messageRecived.Text == "code")
                {
                    string queryCode = "SELECT ValidationCode FROM InnerEncryption WHERE idPlanet = " + planet.GetId();
                    string code = dt.GetByQuery(queryCode).Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                    byte[] encryptedCode = rsa.EncryptedCode(code, planet.GetId().ToString());

                    tcp.SendMessageToServer(encryptedCode, planet.GetIp(), planet.GetPort());
                }

                RecivedMessage(messageRecived.Text);
                validationNextStep();
            }
        } //DETECTA PER INICIAR LA DESCODIFICACIO

        private void validationNextStep()
        {
            if (validations[0])
            {
                btn_SendCode.Enabled = true;
                btn_detectPlanet.ForeColor = Color.Green;
                btn_detectPlanet.FlatAppearance.BorderColor = Color.Green;
                if (validations[1])
                {
                    btn_SendFiles.Enabled = true;
                    btn_SendCode.ForeColor = Color.Green;
                    btn_detectPlanet.FlatAppearance.BorderColor = Color.Green;

                    if (validations[2])
                    {
                        btn_SendFiles.ForeColor = Color.Green;
                        btn_detectPlanet.FlatAppearance.BorderColor = Color.Green;
                    }
                } else
                {
                    //validations = new bool[] { false, false, false };
                    //btn_SendCode.ForeColor = Color.Red;
                }
            } else
            {
                //validations = new bool[] { false, false, false };
                btn_detectPlanet.ForeColor = Color.Red;
                btn_detectPlanet.FlatAppearance.BorderColor = Color.Red;
            }
        }//ACTIVANT BOTONS QUAN VAN PASSANT PROCESSOS DE VALIDACIÓ

        private void getCodeAndSend()
        {
            try
            {
                var sqlParams = new Dictionary<string, dynamic>();
                sqlParams.Add(@"planetid", planet.GetId());
                string code = dt.GetByQuery("SELECT ValidationCode FROM InnerEncryption WHERE idPlanet = @planetid",
                        sqlParams)
                    .Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                byte[] encryptedCode = rsa.EncryptedCode(code, planet.GetId().ToString());

                tcp.SendMessageToServer("VK", planet.GetIp(), planet.GetPort());

                printPanel("[SYSTEM] - Key validation send it, waiting for response...");
            }
            catch
            {
                printPanel("[ERROR] - Key validation error, problem with connecting to server");
            }
        } //DEMANAR LA CLAU I EL CODI A LA BBDD I ENVIARLO

        private void offButton_Click(object sender, EventArgs e)
        {
            this.Close();
        } //TANCA LA PAGINA DE LA NAU

        private void SendCodeButton_Click(object sender, EventArgs e)
        {
            getCodeAndSend();
        } //BOTON PARA ENVIAR EL CODIGO ENCRUPTADO AL PLANETA

        private void WriteTextSafe(string text)
        {
            if (messageRecived.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                messageRecived.Invoke(d, new object[] { text });
            }
            else
            {
                messageRecived.Text = "";
                messageRecived.Text = text;
            }
        }//FUNCIO PER PODER ESCRIURE AL TEXT BOX SENSE QUE PETI EL THREAD

        //BARRA MOVIMENT DE LA PANTALLA
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _lastLocation = e.Location;
        }
        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                //ChangeBorderColor(Color.Red);
                Location = new Point(
                    Location.X - _lastLocation.X + e.X, Location.Y - _lastLocation.Y + e.Y);

                Update();
            }
        }
        private void panel4_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            //ChangeBorderColor(Color.Yellow);
        }

        private void decodeFiles ()
        {
            string extractPath = Application.StartupPath + "\\ZipFiles";
            string downloadPath = Application.StartupPath + "\\Downloads";

            List<string> strFiles = Directory.GetFiles(extractPath, "*", SearchOption.AllDirectories).ToList();

            foreach (string fichero in strFiles)
            {
                File.Delete(fichero);
            }

            ZipFile.ExtractToDirectory(downloadPath, extractPath);

            var res = dt.GetByQuery("SELECT Word, Numbers FROM InnerEncryptionData as IED LEFT JOIN InnerEncryption as IE on IE.idInnerEncryption = IED.IdInnerEncryption WHERE IE.idPlanet = "+planet.GetId()+";");
            Dictionary<string, string> abcCodes = new Dictionary<string, string>();

            for (int i = 0; i < 24; i++)
            {
                var dr = res.Tables[0].Rows[i];
                abcCodes.Add(dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(0).ToString());
            }

            string pathFile = Application.StartupPath + "\\ZipFiles\\PacsSol.txt";

            var iStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(iStream);

            foreach (string file in strFiles) 
            {
                var oStream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Read);
                
                var sw = new StreamWriter(oStream);

                string code;
                string letter;

                while (sr.Peek() >= 0)
                {
                    code = sr.ReadLine();

                    letter = abcCodes[code];

                    sw.WriteLine(letter); sw.Flush();
                }
                sw.Close();
            }
            sr.Close();
        }//FUNCIO A EXECUTAR QUAN LA NAU REP ELS TRES ARXIUS

        private void RecivedFiles()
        {
            while (active)
            {
                String mssg = tcp.RecivedFile(FileListener, Application.StartupPath + "\\Downloads\\");
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

            var res = dt.GetByQuery("SELECT Word, Numbers FROM InnerEncryptionData as IED LEFT JOIN InnerEncryption as IE on IE.idInnerEncryption = IED.IdInnerEncryption WHERE IE.idPlanet = " + planet.GetId() + ";");
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

        }//FUNCIO PER ENVIAR L'ARXIU DESCODIFICAT AL PLANETA
    }
}