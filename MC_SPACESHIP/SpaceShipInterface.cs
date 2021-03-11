using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using PACS_Utils;
using PACS_Objects;
using System.Threading;
using System.Net.Sockets;

namespace MC_SPACESHIP
{
    public partial class SpaceShipInterface : Form
    {
        public SpaceShipInterface()
        {
            InitializeComponent();
        }

        Thread t1;
        bool active;
        DataAccessService dt = new DataAccessService();
        TCPIPSystemService tcp = new TCPIPSystemService();
        TcpListener Listener;
        Planet planet;
        SpaceShip spaceShip;

        private void SpaceShipInterface_Load(object sender, EventArgs e)
        {
            //COMBOBOX AMB ELS PLANETES A ESCOLLIR
            string sqlSpaceShip = "SELECT idSpaceShip, CodeSpaceShip, IPSpaceShip, PortSpaceShip FROM SpaceShips WHERE CodeSpaceShip = 'X-Wing R0001';";
            DataSet ds1 = dt.GetByQuery(sqlSpaceShip);

            var dr = ds1.Tables[0].Rows[0];

            spaceShip = new SpaceShip(Int32.Parse(dr.ItemArray.GetValue(0).ToString()), dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(2).ToString(), Int32.Parse(dr.ItemArray.GetValue(3).ToString()));

            string sql = "SELECT DescPlanet FROM Planets;";
            DataSet ds2 = dt.GetByQuery(sql);

            List<object> Planets = new List<object>();

            foreach (DataRow row in ds2.Tables[0].Rows)
            {
                comboPlanet.Items.Add(row["DescPlanet"]);
            }
        }

        private void ping_Click(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null)
            {
                printPanel(tcp.checkXarxa("8.8.8.8", 5));
            }
        }

        private void comboPlanet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPlanet.SelectedItem != null)
            {
                string sql = "SELECT idPlanet, CodePlanet, DescPlanet, IPPlanet, PortPlanet FROM Planets WHERE DescPlanet = '" + comboPlanet.SelectedItem + "';";
                DataSet ds = dt.GetByQuery(sql);

                var dr = ds.Tables[0].Rows[0];

                planet = new Planet(Int32.Parse(dr.ItemArray.GetValue(0).ToString()), dr.ItemArray.GetValue(1).ToString(), dr.ItemArray.GetValue(2).ToString(), dr.ItemArray.GetValue(3).ToString(), Int32.Parse(dr.ItemArray.GetValue(4).ToString()));

                printPanel("[SYSTEM] - Selected Planet: " + planet.getCode() + " | " + planet.getName() + " - Address: " + planet.getIp() + " - Port: " + planet.getPort() + " - Ready to CHECK");

                string mssg = "ER"+spaceShip.getCode()+"DADAD";

                try
                {
                    //tcp.SendMessageToServer(mssg, planet.getIp(), planet.getPort());
                } catch (Exception ex)
                {
                    printPanel("[ERROR] - Error connecting to Server of the Planet");
                }
            }
        }

        private void printPanel (string message)
        {
            if (message != "")
            {
                if (SpaceShipConsole.Items.Count > 30)
                {
                    SpaceShipConsole.Items.Clear();
                }

                SpaceShipConsole.Items.Add(message);

            } else
            {

            }
        }

        private void StartServer_Click(object sender, EventArgs e)
        {
            active = true;
            tcp.StartServer(spaceShip.getPort(), Listener);
            t1 = new Thread(ListenerServer);
            t1.Start();
        }

        private void ListenerServer()
        {
            string mssg;

            while (active)
            {
                mssg = tcp.WaitingForResponse(Listener);
                if (mssg != null)
                {
                    messageRecived.Text = mssg;

                }
            }
        }

        private void messageRecived_TextChanged(object sender, EventArgs e)
        {
            // FUNCIO DEL POL ON DESXIFRAR MISSATGE ENVIAT PER LA BASE DE DADES
        }
        //DEMANAR LA CLAU I EL CODI A LA BBDD 

        //ENVIAR TCP IP EL CODI ENCRIPTADA
    }
}
