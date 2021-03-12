using System;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new PlanetInterface();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new SpaceShipInterface();
            form.Show();
        }
    }
}