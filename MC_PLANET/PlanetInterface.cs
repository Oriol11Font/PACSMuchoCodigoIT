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
                PrintPanel(
                    $@"[SYSTEM] - Started server for planet {_planet.GetName()}. Listening on port {_planet.GetPort()}");
            }
            else
            {
                // stop thread
                //_listenerThread.Join();
                _tcp = null;
                _listener = null;
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