﻿namespace PACS_CustomControls
{
    partial class CircularButton
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

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictrButton = new System.Windows.Forms.PictureBox();
            this.lblButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictrButton)).BeginInit();
            this.SuspendLayout();
            // 
            // pictrButton
            // 
            this.pictrButton.Location = new System.Drawing.Point(33, 47);
            this.pictrButton.Name = "pictrButton";
            this.pictrButton.Size = new System.Drawing.Size(101, 88);
            this.pictrButton.TabIndex = 0;
            this.pictrButton.TabStop = false;
            this.pictrButton.Click += new System.EventHandler(this.pictrButton_Click);
            // 
            // lblButton
            // 
            this.lblButton.Location = new System.Drawing.Point(3, 27);
            this.lblButton.Name = "lblButton";
            this.lblButton.Size = new System.Drawing.Size(168, 17);
            this.lblButton.TabIndex = 1;
            this.lblButton.Text = "lblBotton";
            // 
            // CircularButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblButton);
            this.Controls.Add(this.pictrButton);
            this.Name = "CircularButton";
            this.Size = new System.Drawing.Size(171, 135);
            ((System.ComponentModel.ISupportInitialize)(this.pictrButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictrButton;
        private System.Windows.Forms.Label lblButton;
    }
}
