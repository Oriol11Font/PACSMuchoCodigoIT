using System;
using System.Windows.Forms;

namespace PACS_CustomControls
{
    public partial class CircularButton: UserControl
    {
        private string buttonON = Application.StartupPath + "\\imgs\\buttonON.png";
        private string buttonOFF = Application.StartupPath + "\\imgs\\buttonOFF.png";

        public string BtnLabel
        {
            get { return lblButton.Text; }
            set { lblButton.Text = value; }
        }

        public CircularButton()
        {
            InitializeComponent();
            lblButton.Text = "OUT";
            pictrButton.SizeMode = PictureBoxSizeMode.StretchImage;
            pictrButton.ImageLocation = buttonOFF;
        }

        

        private void pictrButton_Click(object sender, EventArgs e)
        {
            if (pictrButton.ImageLocation == buttonON)
            {
               pictrButton.ImageLocation = buttonOFF;
            } else
            {
               pictrButton.ImageLocation = buttonON;
            }
        }
    }
}
