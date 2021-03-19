
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
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.onOffButton)).BeginInit();
            this.SuspendLayout();
            // 
            // comboPlanet
            // 
            this.comboPlanet.FormattingEnabled = true;
            this.comboPlanet.Location = new System.Drawing.Point(840, 234);
            this.comboPlanet.Margin = new System.Windows.Forms.Padding(2);
            this.comboPlanet.Name = "comboPlanet";
            this.comboPlanet.Size = new System.Drawing.Size(113, 21);
            this.comboPlanet.TabIndex = 2;
            this.comboPlanet.SelectedIndexChanged += new System.EventHandler(this.comboPlanet_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(840, 314);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 56);
            this.button1.TabIndex = 3;
            this.button1.Text = "Sol·licitar Claus";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(840, 373);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 57);
            this.button2.TabIndex = 4;
            this.button2.Text = "Enviar Codi Verficacio";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.SpaceShipConsole);
            this.panel1.Location = new System.Drawing.Point(103, 102);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(663, 382);
            this.panel1.TabIndex = 5;
            // 
            // SpaceShipConsole
            // 
            this.SpaceShipConsole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(33)))), ((int)(((byte)(71)))));
            this.SpaceShipConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SpaceShipConsole.ForeColor = System.Drawing.SystemColors.Menu;
            this.SpaceShipConsole.FormattingEnabled = true;
            this.SpaceShipConsole.Location = new System.Drawing.Point(10, 10);
            this.SpaceShipConsole.Margin = new System.Windows.Forms.Padding(2);
            this.SpaceShipConsole.Name = "SpaceShipConsole";
            this.SpaceShipConsole.Size = new System.Drawing.Size(640, 351);
            this.SpaceShipConsole.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(840, 257);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 54);
            this.button3.TabIndex = 6;
            this.button3.Text = "Detectar Planeta";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.ping_Click);
            // 
            // messageRecived
            // 
            this.messageRecived.Location = new System.Drawing.Point(856, 510);
            this.messageRecived.Margin = new System.Windows.Forms.Padding(2);
            this.messageRecived.Name = "messageRecived";
            this.messageRecived.Size = new System.Drawing.Size(76, 20);
            this.messageRecived.TabIndex = 8;
            this.messageRecived.TextChanged += new System.EventHandler(this.messageRecived_TextChanged);
            // 
            // onOffButton
            // 
            this.onOffButton.BackColor = System.Drawing.Color.Transparent;
            this.onOffButton.Location = new System.Drawing.Point(856, 59);
            this.onOffButton.Name = "onOffButton";
            this.onOffButton.Size = new System.Drawing.Size(76, 107);
            this.onOffButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.onOffButton.TabIndex = 10;
            this.onOffButton.TabStop = false;
            this.onOffButton.Click += new System.EventHandler(this.StartServer_Click);
            // 
            // SpaceShipInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MC_SPACESHIP.Properties.Resources.BackGroundSpaceShip1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1011, 586);
            this.Controls.Add(this.onOffButton);
            this.Controls.Add(this.messageRecived);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboPlanet);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SpaceShipInterface";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.SpaceShipInterface_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.onOffButton)).EndInit();
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
    }
}

