using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using PACS_Utils;

namespace MC_PLANET
{
    public partial class PlanetInterface : Form
    {
        private readonly RsaKeysService _keysService = new RsaKeysService();
        private readonly DataAccessService _dataAccess = new DataAccessService();

        public PlanetInterface()
        {
            InitializeComponent();
        }

        private void genKey_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "HOLA" + Environment.NewLine;

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
        }


        //LLEGIR CLAU ENVIADA PER LA NAU

        //DESENCRIPTAR LA CLAU
        private void PlanetInterface_Load(object sender, EventArgs e)
        {
            var res = _dataAccess.GetTable(@"Planets");
            planetCmbx.DataSource = res.Tables[0];
            planetCmbx.ValueMember = @"idPlanet";
            planetCmbx.DisplayMember = @"DescPlanet";
        }
    }
}