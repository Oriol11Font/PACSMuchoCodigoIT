using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using PACS_Objects;
using PACS_Utils;

namespace MC_SPACESHIP
{
    public partial class SpaceShipInterface : Form
    {
        private readonly DataAccessService _dt = new DataAccessService();
        private readonly Planet _planet = new Planet();
        private readonly TcpipSystemService _tcp = new TcpipSystemService();

        public SpaceShipInterface()
        {
            InitializeComponent();
        }

        private void SpaceShipInterface_Load(object sender, EventArgs e)
        {
            //COMBOBOX AMB ELS PLANETES A ESCOLLIR
            var sql = "SELECT DescPlanet FROM Planets;";
            var ds = _dt.GetByQuery(sql);

            var planets = new List<object>();

            foreach (DataRow row in ds.Tables[0].Rows) comboPlanet.Items.Add(row["DescPlanet"]);
        }

        private void ping_Click(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null) PrintPanel(_tcp.CheckXarxa(_planet.GetIp(), 5));
        }

        private void comboPlanet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null)
            {
                var sql =
                    "SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet FROM Planets WHERE DescPlanet = '" +
                    comboPlanet.SelectedItem + "';";
                var ds = _dt.GetByQuery(sql);

                var dr = ds.Tables[0].Rows[0];

                _planet.Insert(int.Parse(dr.ItemArray.GetValue(0).ToString()), dr.ItemArray.GetValue(1).ToString(),
                    dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(1).ToString(),
                    dr.ItemArray.GetValue(1).ToString());

                PrintPanel("[INFO] - Contected to " + _planet.GetCode() + " | " + _planet.GetName() + " Address: " +
                           _planet.GetIp() + " - Ready to CHECK");
            }
        }

        private void PrintPanel(string message)
        {
            if (message != "")
            {
                SpaceShipPanel.Text = SpaceShipPanel.Text + message + Environment.NewLine;
            }
        }

        //DEMANAR LA CLAU I EL CODI A LA BBDD 

        //ENVIAR TCP IP EL CODI ENCRIPTADA
    }
}