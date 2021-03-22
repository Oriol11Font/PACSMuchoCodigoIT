using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly RsaKeysService _keysService = new RsaKeysService();
        private readonly DataAccessService _dataAccess = new DataAccessService();
        private TcpipSystemService _tcp;
        private TcpListener _listener;
        private Thread _listenerThread;
        private bool _active;
        private Planet _planet;
        private SpaceShip _spaceShip;

        public PlanetInterface()
        {
            InitializeComponent();
        }

        private void genKey_Click(object sender, EventArgs e)
        {
            var validationCode = _keysService.GenerateKey(12);

            var sqlParams = new Dictionary<string, string>();
            sqlParams.Add("validationcode", validationCode);
            sqlParams.Add("idplanet", planetCmbx.SelectedValue.ToString());
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

            var letters = _keysService.GenerateEncryptedChars();

            sqlParams.Clear();
            sqlParams.Add("idinnerencryption", idinnerencryption.ToString());

            List<string> values = new List<string>();

            foreach (var letterPair in letters)
                values.Add($@"({idinnerencryption}, '{letterPair.Key}', '{letterPair.Value}')");

            _dataAccess.RunSafeQuery(
                $@"BEGIN TRANSACTION; DELETE FROM InnerEncryptionData WHERE idInnerEncryption = @idinnerencryption; INSERT INTO InnerEncryptionData (IdInnerEncryption, Word, Numbers) VALUES {string.Join(", ", values)}; COMMIT TRANSACTION;",
                sqlParams);

            PrintPanel(
                $@"[SYSTEM] Generated Validation Code and encrypted letters for planet {_planet.GetName()}");
        }

        private void PrintPanel(string message)
        {
            if (message != "")
                textBox1.Text = textBox1.Text + message + Environment.NewLine;
        }

        private void ListenerServer()
        {
            while (_active)
            {
                var msg = _tcp.WaitingForResponse(_listener);
                if (msg != null) HandleMessage(msg);
            }
        }

        private void HandleMessage(String msg)
        {
            var msgType = msg.Substring(0, 2).ToUpper();
            switch (msgType)
            {
                case "ER":
                {
                    try
                    {
                        PrintPanel($@"[SYSTEM] Rebut missatge de validació d'entrada: {msg}");

                        if (msg.Length == 26)
                        {
                            var shipCode = msg.Substring(2, 12);
                            var deliveryCode = msg.Substring(14, 12);

                            Dictionary<string, string> sqlParams = new Dictionary<string, string>();

                            sqlParams.Add("codespaceship", shipCode);
                            var spaceshipRows =
                                _dataAccess.GetByQuery(
                                    "SELECT CodeSpaceShip, IPSpaceShip, PortSpaceShip FROM SpaceShips WHERE CodeSpaceShip = @codespaceship",
                                    sqlParams).Tables[0].Rows;
                            var spaceship = spaceshipRows[0].ItemArray;

                            sqlParams.Clear();

                            sqlParams.Add("codedelivery", deliveryCode);
                            var deliveryRows =
                                _dataAccess.GetByQuery("SELECT * FROM DeliveryData WHERE CodeDelivery = @codedelivery",
                                    sqlParams).Tables[0].Rows;
                            var delivery = deliveryRows[0].ItemArray;

                            var response =
                                $@"VR{spaceship[0]}{(spaceshipRows.Count == 1 && deliveryRows.Count == 1 ? "VP" : "AD")}";
                            PrintPanel(response);

                            // codigo valido: ERX-WingsR0001abcdefghijkc

                            _tcp.SendMessageToServer(
                                response,
                                spaceship[1].ToString(),
                                int.Parse(spaceship[2].ToString()));
                        }
                        else
                        {
                            PrintPanel($@"[SYSTEM] El missatge de validació d'entrada no té 26 caràcters de longitud");
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.ToString());
                        PrintPanel($@"[SYSTEM] Error on handling entrance petition");
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

        //LLEGIR CLAU ENVIADA PER LA NAU

        //DESENCRIPTAR LA CLAU
        private void PlanetInterface_Load(object sender, EventArgs e)
        {
            var res = _dataAccess.GetTable(@"Planets");
            planetCmbx.DataSource = res.Tables[0];
            planetCmbx.ValueMember = @"idPlanet";
            planetCmbx.DisplayMember = @"DescPlanet";

            planetCmbx_ValueMemberChanged(null, null);
        }

        private void CircularButton_Click(object sender, EventArgs e)
        {
            _active = !_active;
            if (_active)
            {
                _tcp = new TcpipSystemService();
                _listener = _tcp.StartServer(_planet.GetPort(), _listener);
                _listenerThread = new Thread(ListenerServer);
                _listenerThread.Start();
                PrintPanel(
                    $@"[SYSTEM] - Started server for planet {_planet.GetName()}. Listening on port {_planet.GetPort()}");
            }
            else
            {
                // stop thread
                //_listenerThread.Join();
                _tcp = null;
                _listener = null;
                PrintPanel(
                    @"[SYSTEM] - Stopped server");
            }
        }

        private void planetCmbx_ValueMemberChanged(object sender, EventArgs e)
        {
            Dictionary<string, string> sqlParams = new Dictionary<string, string>();
            sqlParams.Add("idplanet", planetCmbx.SelectedValue.ToString());
            var planetRow = _dataAccess.GetByQuery(
                @"SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet FROM Planets WHERE idPlanet = @idplanet",
                sqlParams).Tables[0].Rows[0].ItemArray;
            _planet = new Planet(int.Parse(planetRow[0].ToString()), planetRow[1].ToString(), planetRow[2].ToString(),
                planetRow[3].ToString(), int.Parse(planetRow[4].ToString()));

            PrintPanel($@"[SYSTEM] - Selected Planet {_planet.GetName()}");
        }
    }
}