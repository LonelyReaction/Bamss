namespace WindowsFormsTester
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.chkCorrection = new System.Windows.Forms.CheckBox();
            this.chkDoubleRead = new System.Windows.Forms.CheckBox();
            this.nudScanTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cameraBarCodeReader1 = new BAMSS.Forms.Control.CameraBarCodeReader();
            ((System.ComponentModel.ISupportInitialize)(this.nudScanTime)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 461);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 47);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(424, 461);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 20);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(115, 461);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 47);
            this.button2.TabIndex = 3;
            this.button2.Text = "End";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(220, 461);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 47);
            this.button3.TabIndex = 4;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(324, 461);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 20);
            this.button4.TabIndex = 5;
            this.button4.Text = "Read";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Location = new System.Drawing.Point(324, 487);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(88, 20);
            this.button5.TabIndex = 6;
            this.button5.Text = "Read";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // chkCorrection
            // 
            this.chkCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCorrection.AutoSize = true;
            this.chkCorrection.Location = new System.Drawing.Point(23, 425);
            this.chkCorrection.Name = "chkCorrection";
            this.chkCorrection.Size = new System.Drawing.Size(77, 16);
            this.chkCorrection.TabIndex = 7;
            this.chkCorrection.Text = "Correction";
            this.chkCorrection.UseVisualStyleBackColor = true;
            this.chkCorrection.CheckedChanged += new System.EventHandler(this.chkCorrection_CheckedChanged);
            // 
            // chkDoubleRead
            // 
            this.chkDoubleRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDoubleRead.AutoSize = true;
            this.chkDoubleRead.Location = new System.Drawing.Point(119, 426);
            this.chkDoubleRead.Name = "chkDoubleRead";
            this.chkDoubleRead.Size = new System.Drawing.Size(85, 16);
            this.chkDoubleRead.TabIndex = 8;
            this.chkDoubleRead.Text = "DoubleRead";
            this.chkDoubleRead.UseVisualStyleBackColor = true;
            this.chkDoubleRead.CheckedChanged += new System.EventHandler(this.chkDoubleCheck_CheckedChanged);
            // 
            // nudScanTime
            // 
            this.nudScanTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudScanTime.Location = new System.Drawing.Point(291, 424);
            this.nudScanTime.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudScanTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScanTime.Name = "nudScanTime";
            this.nudScanTime.Size = new System.Drawing.Size(88, 19);
            this.nudScanTime.TabIndex = 9;
            this.nudScanTime.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudScanTime.ValueChanged += new System.EventHandler(this.nudScanTime_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 429);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "ScanTime";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // cameraBarCodeReader1
            // 
            this.cameraBarCodeReader1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraBarCodeReader1.CameraNo = 1;
            this.cameraBarCodeReader1.Correction = false;
            this.cameraBarCodeReader1.DoubleRead = true;
            this.cameraBarCodeReader1.Location = new System.Drawing.Point(12, 12);
            this.cameraBarCodeReader1.Name = "cameraBarCodeReader1";
            this.cameraBarCodeReader1.ReadScanTime = ((long)(30000));
            this.cameraBarCodeReader1.Size = new System.Drawing.Size(552, 397);
            this.cameraBarCodeReader1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.cameraBarCodeReader1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 520);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudScanTime);
            this.Controls.Add(this.chkDoubleRead);
            this.Controls.Add(this.chkCorrection);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cameraBarCodeReader1);
            this.Name = "Form1";
            this.Text = "This is Tester for Barcode Reader.";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudScanTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BAMSS.Forms.Control.CameraBarCodeReader cameraBarCodeReader1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox chkCorrection;
        private System.Windows.Forms.CheckBox chkDoubleRead;
        private System.Windows.Forms.NumericUpDown nudScanTime;
        private System.Windows.Forms.Label label1;
    }
}

