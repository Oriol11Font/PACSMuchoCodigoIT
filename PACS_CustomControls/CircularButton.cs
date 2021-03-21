using System;
using System.Windows.Forms;

namespace PACS_CustomControls
{
    public partial class CircularButton : UserControl
    {
<<<<<<< HEAD
        private readonly string _buttonOff = Application.StartupPath + "\\imgs\\buttonOFF.png";
        private readonly string _buttonOn = Application.StartupPath + "\\imgs\\buttonON.png";
=======
        private string buttonON = Application.StartupPath + "\\imgs\\buttonON.png";
        private string buttonOFF = Application.StartupPath + "\\imgs\\buttonOFF.png";
>>>>>>> master

        public CircularButton()
        {
            InitializeComponent();
            pictrButton.SizeMode = PictureBoxSizeMode.StretchImage;
            pictrButton.ImageLocation = _buttonOff;
        }


        private void pictrButton_Click(object sender, EventArgs e)
        {
            if (pictrButton.ImageLocation == _buttonOn)
                pictrButton.ImageLocation = _buttonOff;
            else
                pictrButton.ImageLocation = _buttonOn;
        }
    }
}