namespace BAMSS.Process
{
    partial class RoopWatcherForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoopWatcherForm));
            this.timProcess = new System.Windows.Forms.Timer(this.components);
            this.lblScanName2 = new System.Windows.Forms.Label();
            this.lblScanName1 = new System.Windows.Forms.Label();
            this.picScanSignal3 = new System.Windows.Forms.PictureBox();
            this.picScanSignal2 = new System.Windows.Forms.PictureBox();
            this.picScanSignal1 = new System.Windows.Forms.PictureBox();
            this.imlSignals = new System.Windows.Forms.ImageList(this.components);
            this.lblScanName3 = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonMini = new System.Windows.Forms.Button();
            this.progressProcess = new System.Windows.Forms.ToolStripProgressBar();
            this.labelProcessStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProcess = new System.Windows.Forms.StatusStrip();
            this.buttonCommunicationForceReset = new System.Windows.Forms.Button();
            this.labelProcessName = new System.Windows.Forms.Label();
            this.radioPausing = new System.Windows.Forms.RadioButton();
            this.radioRunning = new System.Windows.Forms.RadioButton();
            this.labelVersion = new System.Windows.Forms.Label();
            this.gboxRunningStatus = new System.Windows.Forms.GroupBox();
            this.lstInfo = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.picScanSignal3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picScanSignal2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picScanSignal1)).BeginInit();
            this.statusProcess.SuspendLayout();
            this.gboxRunningStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // timProcess
            // 
            this.timProcess.Interval = 1000;
            // 
            // lblScanName2
            // 
            this.lblScanName2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScanName2.Location = new System.Drawing.Point(344, 53);
            this.lblScanName2.Name = "lblScanName2";
            this.lblScanName2.Size = new System.Drawing.Size(174, 24);
            this.lblScanName2.TabIndex = 27;
            this.lblScanName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScanName1
            // 
            this.lblScanName1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScanName1.Location = new System.Drawing.Point(344, 28);
            this.lblScanName1.Name = "lblScanName1";
            this.lblScanName1.Size = new System.Drawing.Size(174, 24);
            this.lblScanName1.TabIndex = 26;
            this.lblScanName1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picScanSignal3
            // 
            this.picScanSignal3.Location = new System.Drawing.Point(319, 80);
            this.picScanSignal3.Name = "picScanSignal3";
            this.picScanSignal3.Size = new System.Drawing.Size(20, 20);
            this.picScanSignal3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picScanSignal3.TabIndex = 25;
            this.picScanSignal3.TabStop = false;
            // 
            // picScanSignal2
            // 
            this.picScanSignal2.Location = new System.Drawing.Point(319, 55);
            this.picScanSignal2.Name = "picScanSignal2";
            this.picScanSignal2.Size = new System.Drawing.Size(20, 20);
            this.picScanSignal2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picScanSignal2.TabIndex = 24;
            this.picScanSignal2.TabStop = false;
            // 
            // picScanSignal1
            // 
            this.picScanSignal1.Location = new System.Drawing.Point(319, 30);
            this.picScanSignal1.Name = "picScanSignal1";
            this.picScanSignal1.Size = new System.Drawing.Size(20, 20);
            this.picScanSignal1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picScanSignal1.TabIndex = 23;
            this.picScanSignal1.TabStop = false;
            // 
            // imlSignals
            // 
            this.imlSignals.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSignals.ImageStream")));
            this.imlSignals.TransparentColor = System.Drawing.Color.Transparent;
            this.imlSignals.Images.SetKeyName(0, "Idle.png");
            this.imlSignals.Images.SetKeyName(1, "Executing.png");
            // 
            // lblScanName3
            // 
            this.lblScanName3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScanName3.Location = new System.Drawing.Point(344, 78);
            this.lblScanName3.Name = "lblScanName3";
            this.lblScanName3.Size = new System.Drawing.Size(174, 24);
            this.lblScanName3.TabIndex = 28;
            this.lblScanName3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonExit
            // 
            this.buttonExit.Enabled = false;
            this.buttonExit.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonExit.ForeColor = System.Drawing.Color.Red;
            this.buttonExit.Location = new System.Drawing.Point(120, 79);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(113, 24);
            this.buttonExit.TabIndex = 22;
            this.buttonExit.Text = "終　了";
            this.buttonExit.UseVisualStyleBackColor = true;
            // 
            // buttonMini
            // 
            this.buttonMini.Location = new System.Drawing.Point(120, 39);
            this.buttonMini.Name = "buttonMini";
            this.buttonMini.Size = new System.Drawing.Size(113, 34);
            this.buttonMini.TabIndex = 21;
            this.buttonMini.Text = "最小化";
            this.buttonMini.UseVisualStyleBackColor = true;
            // 
            // progressProcess
            // 
            this.progressProcess.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressProcess.AutoSize = false;
            this.progressProcess.BackColor = System.Drawing.SystemColors.Control;
            this.progressProcess.Name = "progressProcess";
            this.progressProcess.Size = new System.Drawing.Size(100, 22);
            this.progressProcess.Step = 20;
            this.progressProcess.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // labelProcessStatus
            // 
            this.labelProcessStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelProcessStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.labelProcessStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.labelProcessStatus.Name = "labelProcessStatus";
            this.labelProcessStatus.Size = new System.Drawing.Size(50, 23);
            this.labelProcessStatus.Text = "Status";
            // 
            // statusProcess
            // 
            this.statusProcess.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusProcess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelProcessStatus,
            this.progressProcess});
            this.statusProcess.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusProcess.Location = new System.Drawing.Point(0, 259);
            this.statusProcess.Name = "statusProcess";
            this.statusProcess.Size = new System.Drawing.Size(522, 28);
            this.statusProcess.SizingGrip = false;
            this.statusProcess.TabIndex = 19;
            // 
            // buttonCommunicationForceReset
            // 
            this.buttonCommunicationForceReset.BackColor = System.Drawing.Color.Red;
            this.buttonCommunicationForceReset.Enabled = false;
            this.buttonCommunicationForceReset.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonCommunicationForceReset.ForeColor = System.Drawing.Color.Yellow;
            this.buttonCommunicationForceReset.Location = new System.Drawing.Point(239, 39);
            this.buttonCommunicationForceReset.Name = "buttonCommunicationForceReset";
            this.buttonCommunicationForceReset.Size = new System.Drawing.Size(74, 64);
            this.buttonCommunicationForceReset.TabIndex = 18;
            this.buttonCommunicationForceReset.Text = "状態強制\r\nリセット";
            this.buttonCommunicationForceReset.UseVisualStyleBackColor = false;
            // 
            // labelProcessName
            // 
            this.labelProcessName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProcessName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelProcessName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelProcessName.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelProcessName.Location = new System.Drawing.Point(5, 3);
            this.labelProcessName.Name = "labelProcessName";
            this.labelProcessName.Size = new System.Drawing.Size(513, 22);
            this.labelProcessName.TabIndex = 17;
            this.labelProcessName.Text = "プロセス名";
            this.labelProcessName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioPausing
            // 
            this.radioPausing.AutoSize = true;
            this.radioPausing.Location = new System.Drawing.Point(37, 40);
            this.radioPausing.Name = "radioPausing";
            this.radioPausing.Size = new System.Drawing.Size(59, 16);
            this.radioPausing.TabIndex = 1;
            this.radioPausing.Text = "停止中";
            this.radioPausing.UseVisualStyleBackColor = true;
            // 
            // radioRunning
            // 
            this.radioRunning.AutoSize = true;
            this.radioRunning.Checked = true;
            this.radioRunning.Location = new System.Drawing.Point(37, 19);
            this.radioRunning.Name = "radioRunning";
            this.radioRunning.Size = new System.Drawing.Size(59, 16);
            this.radioRunning.TabIndex = 0;
            this.radioRunning.TabStop = true;
            this.radioRunning.Text = "動作中";
            this.radioRunning.UseVisualStyleBackColor = true;
            // 
            // labelVersion
            // 
            this.labelVersion.Location = new System.Drawing.Point(239, 27);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(74, 12);
            this.labelVersion.TabIndex = 20;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gboxRunningStatus
            // 
            this.gboxRunningStatus.Controls.Add(this.radioPausing);
            this.gboxRunningStatus.Controls.Add(this.radioRunning);
            this.gboxRunningStatus.Enabled = false;
            this.gboxRunningStatus.Location = new System.Drawing.Point(5, 33);
            this.gboxRunningStatus.Name = "gboxRunningStatus";
            this.gboxRunningStatus.Size = new System.Drawing.Size(109, 70);
            this.gboxRunningStatus.TabIndex = 16;
            this.gboxRunningStatus.TabStop = false;
            this.gboxRunningStatus.Text = "動作";
            // 
            // lstInfo
            // 
            this.lstInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInfo.FormattingEnabled = true;
            this.lstInfo.ItemHeight = 12;
            this.lstInfo.Location = new System.Drawing.Point(5, 106);
            this.lstInfo.Name = "lstInfo";
            this.lstInfo.Size = new System.Drawing.Size(513, 148);
            this.lstInfo.TabIndex = 29;
            // 
            // RoopWatcherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 287);
            this.Controls.Add(this.lstInfo);
            this.Controls.Add(this.lblScanName2);
            this.Controls.Add(this.lblScanName1);
            this.Controls.Add(this.picScanSignal3);
            this.Controls.Add(this.picScanSignal2);
            this.Controls.Add(this.picScanSignal1);
            this.Controls.Add(this.lblScanName3);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonMini);
            this.Controls.Add(this.statusProcess);
            this.Controls.Add(this.buttonCommunicationForceReset);
            this.Controls.Add(this.labelProcessName);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.gboxRunningStatus);
            this.Name = "RoopWatcherForm";
            this.Text = "RoopWatcherForm";
            ((System.ComponentModel.ISupportInitialize)(this.picScanSignal3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picScanSignal2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picScanSignal1)).EndInit();
            this.statusProcess.ResumeLayout(false);
            this.statusProcess.PerformLayout();
            this.gboxRunningStatus.ResumeLayout(false);
            this.gboxRunningStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Timer timProcess;
        internal System.Windows.Forms.Label lblScanName2;
        internal System.Windows.Forms.Label lblScanName1;
        internal System.Windows.Forms.PictureBox picScanSignal3;
        internal System.Windows.Forms.PictureBox picScanSignal2;
        internal System.Windows.Forms.PictureBox picScanSignal1;
        internal System.Windows.Forms.ImageList imlSignals;
        internal System.Windows.Forms.Label lblScanName3;
        internal System.Windows.Forms.Button buttonExit;
        internal System.Windows.Forms.Button buttonMini;
        protected System.Windows.Forms.ToolStripProgressBar progressProcess;
        private System.Windows.Forms.ToolStripStatusLabel labelProcessStatus;
        private System.Windows.Forms.StatusStrip statusProcess;
        private System.Windows.Forms.Button buttonCommunicationForceReset;
        protected System.Windows.Forms.Label labelProcessName;
        private System.Windows.Forms.RadioButton radioPausing;
        private System.Windows.Forms.RadioButton radioRunning;
        internal System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.GroupBox gboxRunningStatus;
        private System.Windows.Forms.ListBox lstInfo;

    }
}