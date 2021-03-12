using System;
using System.Windows.Forms;

namespace PACS_CustomControls
{
    public partial class CircularButton : UserControl
    {
        private readonly string _buttonOff = Application.StartupPath + "\\imgs\\buttonOFF.png";
        private readonly string _buttonOn = Application.StartupPath + "\\imgs\\buttonON.png";

        public CircularButton()
        {
            InitializeComponent();
            lblButton.Text = "OUT";
            pictrButton.SizeMode = PictureBoxSizeMode.StretchImage;
            pictrButton.ImageLocation = _buttonOff;
        }

        public string BtnLabel
        {
            get => lblButton.Text;
            set => lblButton.Text = value;
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