using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PACS_MuchoCodigoIT
{
    public partial class user_screen : Form
    {
        public user_screen()
        {
            InitializeComponent();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Form1 Form1 = new Form1(false);
            Form1.Show();
            this.Close();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Form1 Form1 = new Form1(true);
            Form1.Show();
            this.Close();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
