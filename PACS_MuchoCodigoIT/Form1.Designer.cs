
namespace PACS_MuchoCodigoIT
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.SpaceShipImg = new System.Windows.Forms.PictureBox();
            this.PlanetImg = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SpaceShipImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlanetImg)).BeginInit();
            this.SuspendLayout();
            // 
            // SpaceShipImg
            // 
            this.SpaceShipImg.Location = new System.Drawing.Point(507, 116);
            this.SpaceShipImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SpaceShipImg.Name = "SpaceShipImg";
            this.SpaceShipImg.Size = new System.Drawing.Size(200, 185);
            this.SpaceShipImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SpaceShipImg.TabIndex = 2;
            this.SpaceShipImg.TabStop = false;
            this.SpaceShipImg.Click += new System.EventHandler(this.SpaceShipImg_Click);
            // 
            // PlanetImg
            // 
            this.PlanetImg.Location = new System.Drawing.Point(89, 116);
            this.PlanetImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PlanetImg.Name = "PlanetImg";
            this.PlanetImg.Size = new System.Drawing.Size(200, 185);
            this.PlanetImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PlanetImg.TabIndex = 3;
            this.PlanetImg.TabStop = false;
            this.PlanetImg.Click += new System.EventHandler(this.PlanetImg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(316, 342);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 39);
            this.label1.TabIndex = 4;
            this.label1.Text = "CANCEL";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(80)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PlanetImg);
            this.Controls.Add(this.SpaceShipImg);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(80)))), ((int)(((byte)(92)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpaceShipImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlanetImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox SpaceShipImg;
        private System.Windows.Forms.PictureBox PlanetImgPlanetImg;
        private System.Windows.Forms.PictureBox PlanetImg;
        private System.Windows.Forms.Label label1;
    }
}

