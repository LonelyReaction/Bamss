namespace BAMSS.Forms.Control
{
    partial class CameraBarCodeReadDialogSimple
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraBarCodeReadDialogSimple));
            this.cbrBarCode = new BAMSS.Forms.Control.CameraBarCodeReader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // cbrBarCode
            // 
            this.cbrBarCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbrBarCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.cbrBarCode.CameraNo = 0;
            this.cbrBarCode.Correction = false;
            this.cbrBarCode.DoubleRead = true;
            this.cbrBarCode.Location = new System.Drawing.Point(12, 12);
            this.cbrBarCode.Name = "cbrBarCode";
            this.cbrBarCode.ReadScanTime = ((long)(5000));
            this.cbrBarCode.Size = new System.Drawing.Size(341, 284);
            this.cbrBarCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.cbrBarCode.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // CameraBarCodeReadDialogSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 308);
            this.Controls.Add(this.cbrBarCode);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "CameraBarCodeReadDialogSimple";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BarCode Reader";
            this.Shown += new System.EventHandler(this.CameraBarCodeReadDialog_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private CameraBarCodeReader cbrBarCode;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}