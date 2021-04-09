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
    public partial class splash_screen : Form
    {
        public splash_screen()
        {
            InitializeComponent();
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            user_screen Form1 = new user_screen();
            Form1.Show();
            this.Hide();
            timer1.Stop();
        }

        private void splash_screen_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Application.StartupPath + "\\imgs\\splash_xp.gif";
        }
    }
}
