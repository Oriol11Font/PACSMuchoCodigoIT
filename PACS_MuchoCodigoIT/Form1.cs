using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MC_PLANET;
using MC_SPACESHIP;

namespace PACS_MuchoCodigoIT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PlanetImg.ImageLocation = Application.StartupPath + "\\imgs\\Planet.png";
            SpaceShipImg.ImageLocation = Application.StartupPath + "\\imgs\\SpaceShip.png";
        }

        private void PlanetImg_Click(object sender, EventArgs e)
        {
            PlanetInterface form = new PlanetInterface();
            form.Show();
        }

        private void SpaceShipImg_Click(object sender, EventArgs e)
        {
            SpaceShipInterface form = new SpaceShipInterface();
            form.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
