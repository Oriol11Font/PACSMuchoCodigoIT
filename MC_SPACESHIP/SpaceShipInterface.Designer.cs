
namespace MC_SPACESHIP
{
    partial class SpaceShipInterface
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboPlanet = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SpaceShipConsole = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.messageRecived = new System.Windows.Forms.TextBox();
            this.onOffButton = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.infoSpaceShip = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.onOffButton)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboPlanet
            // 
            this.comboPlanet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPlanet.FormattingEnabled = true;
            this.comboPlanet.Location = new System.Drawing.Point(1120, 180);
            this.comboPlanet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboPlanet.Name = "comboPlanet";
            this.comboPlanet.Size = new System.Drawing.Size(149, 24);
            this.comboPlanet.TabIndex = 2;
            this.comboPlanet.SelectedIndexChanged += new System.EventHandler(this.comboPlanet_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(3, 72);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 69);
            this.button1.TabIndex = 3;
            this.button1.Text = "Sol·licitar Claus";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(3, 145);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 70);
            this.button2.TabIndex = 4;
            this.button2.Text = "Enviar Codi Verficacio";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.SpaceShipConsole);
            this.panel1.Location = new System.Drawing.Point(137, 126);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(883, 469);
            this.panel1.TabIndex = 5;
            // 
            // SpaceShipConsole
            // 
            this.SpaceShipConsole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(33)))), ((int)(((byte)(71)))));
            this.SpaceShipConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SpaceShipConsole.ForeColor = System.Drawing.SystemColors.Menu;
            this.SpaceShipConsole.FormattingEnabled = true;
            this.SpaceShipConsole.ItemHeight = 16;
            this.SpaceShipConsole.Location = new System.Drawing.Point(13, 12);
            this.SpaceShipConsole.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpaceShipConsole.Name = "SpaceShipConsole";
            this.SpaceShipConsole.Size = new System.Drawing.Size(853, 432);
            this.SpaceShipConsole.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(3, 2);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(139, 66);
            this.button3.TabIndex = 6;
            this.button3.Text = "Detectar Planeta";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.ping_Click);
            // 
            // messageRecived
            // 
            this.messageRecived.Location = new System.Drawing.Point(1093, 627);
            this.messageRecived.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.messageRecived.Name = "messageRecived";
            this.messageRecived.Size = new System.Drawing.Size(203, 22);
            this.messageRecived.TabIndex = 8;
            this.messageRecived.Visible = false;
            this.messageRecived.TextChanged += new System.EventHandler(this.messageRecived_TextChanged);
            // 
            // onOffButton
            // 
            this.onOffButton.BackColor = System.Drawing.Color.Transparent;
            this.onOffButton.Location = new System.Drawing.Point(1120, 72);
            this.onOffButton.Margin = new System.Windows.Forms.Padding(4);
            this.onOffButton.Name = "onOffButton";
            this.onOffButton.Size = new System.Drawing.Size(149, 132);
            this.onOffButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.onOffButton.TabIndex = 10;
            this.onOffButton.TabStop = false;
            this.onOffButton.Click += new System.EventHandler(this.StartServer_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(33)))), ((int)(((byte)(71)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.infoSpaceShip);
            this.panel2.Location = new System.Drawing.Point(1120, 210);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(149, 82);
            this.panel2.TabIndex = 6;
            // 
            // infoSpaceShip
            // 
            this.infoSpaceShip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(33)))), ((int)(((byte)(71)))));
            this.infoSpaceShip.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.infoSpaceShip.ForeColor = System.Drawing.SystemColors.Menu;
            this.infoSpaceShip.FormattingEnabled = true;
            this.infoSpaceShip.ItemHeight = 16;
            this.infoSpaceShip.Location = new System.Drawing.Point(-2, 6);
            this.infoSpaceShip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.infoSpaceShip.Name = "infoSpaceShip";
            this.infoSpaceShip.Size = new System.Drawing.Size(149, 64);
            this.infoSpaceShip.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(33)))), ((int)(((byte)(71)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Location = new System.Drawing.Point(1120, 296);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(149, 222);
            this.panel3.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::MC_SPACESHIP.Properties.Resources.powerOff;
            this.pictureBox1.Location = new System.Drawing.Point(1155, 548);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(83, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // SpaceShipInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MC_SPACESHIP.Properties.Resources.BackGroundSpaceShip1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1348, 721);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.messageRecived);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboPlanet);
            this.Controls.Add(this.onOffButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SpaceShipInterface";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.SpaceShipInterface_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.onOffButton)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboPlanet;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox messageRecived;
        private System.Windows.Forms.ListBox SpaceShipConsole;
        private System.Windows.Forms.PictureBox onOffButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox infoSpaceShip;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

