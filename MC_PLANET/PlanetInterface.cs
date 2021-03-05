using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MC_PLANET
{
    public partial class PlanetInterface : Form
    {
        public PlanetInterface()
        {
            InitializeComponent();
        }

        //GENERAR CODI
        private void CreateKey()
        {
            
        }

        private void genKey_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "HOLA" + Environment.NewLine;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //LLEGIR CLAU ENVIADA PER LA NAU

        //DESNCRIPTAR LA CLAU


    }
}
