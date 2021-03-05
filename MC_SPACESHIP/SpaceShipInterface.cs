using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using PACS_Utils;
using PACS_Objects;

namespace MC_SPACESHIP
{
    public partial class SpaceShipInterface : Form
    {
        public SpaceShipInterface()
        {
            InitializeComponent();
        }

        DataAccessService dt = new DataAccessService();
        TCPIPSystemService tcp = new TCPIPSystemService();
        Planet planet = new Planet();

        private void SpaceShipInterface_Load(object sender, EventArgs e)
        {
            //COMBOBOX AMB ELS PLANETES A ESCOLLIR
            string sql = "SELECT DescPlanet FROM Planets;";
            DataSet ds = dt.GetByQuery(sql);

            List<object> Planets = new List<object>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                comboPlanet.Items.Add(row["DescPlanet"]);
            }
        }

        private void ping_Click(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null)
            {
                printPanel(tcp.checkXarxa(planet.getIp(), 5));
            }
        }

        private void comboPlanet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null)
            {
                string sql = "SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet FROM Planets WHERE DescPlanet = '" + comboPlanet.SelectedItem + "';";
                DataSet ds = dt.GetByQuery(sql);

                var dr = ds.Tables[0].Rows[0];

                planet.insert(Int32.Parse(dr.ItemArray.GetValue(0).ToString()), dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(1).ToString());

                printPanel("[INFO] - Contected to " + planet.getCode() + " | " + planet.getName() + " Address: " + planet.getIp() +" - Ready to CHECK");
            }
        }

        private void printPanel (string message)
        {
            if (message != "")
            {
                SpaceShipPanel.Text = SpaceShipPanel.Text + message + Environment.NewLine;
            } else
            {

            }
        }

        //DEMANAR LA CLAU I EL CODI A LA BBDD 

        //ENVIAR TCP IP EL CODI ENCRIPTADA
    }
}
