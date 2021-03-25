
namespace MC_PLANET
{
    partial class PlanetInterface
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanetInterface));
            this.panel1 = new System.Windows.Forms.Panel();
            this.PlanetConsole = new System.Windows.Forms.ListBox();
            this.planetCmbx = new System.Windows.Forms.ComboBox();
            this.onOffButton = new System.Windows.Forms.PictureBox();
            this.txtb_msg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.OffButton = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.onOffButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffButton)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.PlanetConsole);
            this.panel1.Location = new System.Drawing.Point(124, 93);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(582, 538);
            this.panel1.TabIndex = 4;
            // 
            // PlanetConsole
            // 
            this.PlanetConsole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(36)))), ((int)(((byte)(39)))));
            this.PlanetConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PlanetConsole.ForeColor = System.Drawing.SystemColors.Menu;
            this.PlanetConsole.FormattingEnabled = true;
            this.PlanetConsole.ItemHeight = 16;
            this.PlanetConsole.Location = new System.Drawing.Point(13, 11);
            this.PlanetConsole.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PlanetConsole.Name = "PlanetConsole";
            this.PlanetConsole.Size = new System.Drawing.Size(550, 512);
            this.PlanetConsole.TabIndex = 1;
            // 
            // planetCmbx
            // 
            this.planetCmbx.FormattingEnabled = true;
            this.planetCmbx.Location = new System.Drawing.Point(947, 358);
            this.planetCmbx.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.planetCmbx.Name = "planetCmbx";
            this.planetCmbx.Size = new System.Drawing.Size(231, 24);
            this.planetCmbx.TabIndex = 5;
            this.planetCmbx.SelectionChangeCommitted += new System.EventHandler(this.planetCmbx_ValueMemberChanged);
            // 
            // onOffButton
            // 
            this.onOffButton.BackColor = System.Drawing.Color.Transparent;
            this.onOffButton.Location = new System.Drawing.Point(986, 170);
            this.onOffButton.Margin = new System.Windows.Forms.Padding(4);
            this.onOffButton.Name = "onOffButton";
            this.onOffButton.Size = new System.Drawing.Size(149, 132);
            this.onOffButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.onOffButton.TabIndex = 11;
            this.onOffButton.TabStop = false;
            this.onOffButton.Click += new System.EventHandler(this.OnOffButton_Click);
            // 
            // txtb_msg
            // 
            this.txtb_msg.Location = new System.Drawing.Point(989, 546);
            this.txtb_msg.Name = "txtb_msg";
            this.txtb_msg.Size = new System.Drawing.Size(146, 22);
            this.txtb_msg.TabIndex = 12;
            this.txtb_msg.TextChanged += new System.EventHandler(this.txtb_msg_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft PhagsPa", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(947, 334);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 22);
            this.label1.TabIndex = 13;
            this.label1.Text = "Planeta Actual";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(989, 421);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 81);
            this.button1.TabIndex = 14;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OffButton
            // 
            this.OffButton.BackColor = System.Drawing.Color.Transparent;
            this.OffButton.Image = global::MC_PLANET.Properties.Resources.powerOff;
            this.OffButton.Location = new System.Drawing.Point(766, 557);
            this.OffButton.Name = "OffButton";
            this.OffButton.Size = new System.Drawing.Size(83, 74);
            this.OffButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OffButton.TabIndex = 15;
            this.OffButton.TabStop = false;
            this.OffButton.Click += new System.EventHandler(this.OffButton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1347, 32);
            this.panel2.TabIndex = 16;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel4_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel4_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel4_MouseUp);
            // 
            // PlanetInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1348, 721);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.OffButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtb_msg);
            this.Controls.Add(this.onOffButton);
            this.Controls.Add(this.planetCmbx);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PlanetInterface";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.PlanetInterface_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.onOffButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.ComboBox planetCmbx;

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox onOffButton;
        private System.Windows.Forms.ListBox PlanetConsole;
        private System.Windows.Forms.TextBox txtb_msg;
        private System.Windows.Forms.PictureBox OffButton;
        private System.Windows.Forms.Panel panel2;
    }
}

