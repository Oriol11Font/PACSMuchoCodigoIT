using System;
using System.IO;
using System.Windows.Forms;
using MC_PLANET;
using MC_SPACESHIP;

namespace PACS_MuchoCodigoIT
{
    public partial class Form1 : Form
    {
        bool usuari;
        public Form1(bool user)
        {
            InitializeComponent();
            usuari = user;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            PlanetImg.ImageLocation = Path.Combine(Application.StartupPath, "imgs", "Planet.png");
            SpaceShipImg.ImageLocation = Path.Combine(Application.StartupPath, "imgs", "SpaceShip.png");

            if (usuari)
            {
                panel6.Visible = true;
            }
            else
            {
                panel7.Visible = true;
            }
        }

        private void PlanetImg_Click(object sender, EventArgs e)
        {
            var form = new PlanetInterface();
            form.Show();
        }

        private void SpaceShipImg_Click(object sender, EventArgs e)
        {
            var form = new SpaceShipInterface();
            form.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}