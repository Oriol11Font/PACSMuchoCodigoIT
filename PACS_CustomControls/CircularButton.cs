using System;
using System.Windows.Forms;

namespace PACS_CustomControls
{
    public partial class CircularButton: Button
    {
        private string buttonON = Application.StartupPath + "\\imgs\\buttonON.png";
        private string buttonOFF = Application.StartupPath + "\\imgs\\buttonOFF.png";

        public CircularButton()
        {
            InitializeComponent();
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
