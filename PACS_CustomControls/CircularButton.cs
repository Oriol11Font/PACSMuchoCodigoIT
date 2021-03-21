using System;
using System.Windows.Forms;

namespace PACS_CustomControls
{
    public partial class CircularButton : UserControl
    {
        private readonly string _buttonOff = Application.StartupPath + "\\imgs\\buttonOFF.png";
        private readonly string _buttonOn = Application.StartupPath + "\\imgs\\buttonON.png";
        public event EventHandler HandleClick;

        public CircularButton()
        {
            InitializeComponent();
            pictrButton.SizeMode = PictureBoxSizeMode.StretchImage;
            pictrButton.ImageLocation = _buttonOff;
        }


        private void pictrButton_Click(object sender, EventArgs e)
        {
            pictrButton.ImageLocation = pictrButton.ImageLocation == _buttonOn ? _buttonOff : _buttonOn;
            if (HandleClick != null) HandleClick(this, e);
        }
    }
}