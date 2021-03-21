
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.genKey = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.circularButton1 = new PACS_CustomControls.CircularButton();
            this.planetCmbx = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (29)))), ((int) (((byte) (36)))), ((int) (((byte) (39)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.textBox1.Location = new System.Drawing.Point(21, 16);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(493, 446);
            this.textBox1.TabIndex = 0;
            // 
            // genKey
            // 
            this.genKey.Location = new System.Drawing.Point(976, 492);
            this.genKey.Margin = new System.Windows.Forms.Padding(4);
            this.genKey.Name = "genKey";
            this.genKey.Size = new System.Drawing.Size(171, 66);
            this.genKey.TabIndex = 3;
            this.genKey.Text = "Generate Key and encrypted letters";
            this.genKey.UseVisualStyleBackColor = true;
            this.genKey.Click += new System.EventHandler(this.genKey_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(147, 114);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(536, 485);
            this.panel1.TabIndex = 4;
            // 
            // circularButton1
            // 
            this.circularButton1.BackColor = System.Drawing.Color.Transparent;
            this.circularButton1.BtnLabel = "OUT";
            this.circularButton1.Location = new System.Drawing.Point(979, 132);
            this.circularButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.circularButton1.Name = "circularButton1";
            this.circularButton1.Size = new System.Drawing.Size(169, 158);
            this.circularButton1.TabIndex = 2;
            // 
            // planetCmbx
            // 
            this.planetCmbx.FormattingEnabled = true;
            this.planetCmbx.Location = new System.Drawing.Point(976, 461);
            this.planetCmbx.Name = "planetCmbx";
            this.planetCmbx.Size = new System.Drawing.Size(171, 24);
            this.planetCmbx.TabIndex = 5;
            // 
            // PlanetInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1348, 721);
            this.Controls.Add(this.planetCmbx);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.genKey);
            this.Controls.Add(this.circularButton1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PlanetInterface";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.PlanetInterface_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox planetCmbx;

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private PACS_CustomControls.CircularButton circularButton1;
        private System.Windows.Forms.Button genKey;
        private System.Windows.Forms.Panel panel1;
    }
}

