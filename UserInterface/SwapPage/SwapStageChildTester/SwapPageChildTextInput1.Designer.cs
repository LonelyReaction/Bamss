namespace BAMSS.UI.SwapPage
{
    partial class SwapPageChildTextInput1
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle1 = new System.Windows.Forms.Label();
            this.txtSettingValue1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblTitle1
            // 
            this.lblTitle1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitle1.AutoSize = true;
            this.lblTitle1.Location = new System.Drawing.Point(19, 34);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.Size = new System.Drawing.Size(47, 12);
            this.lblTitle1.TabIndex = 0;
            this.lblTitle1.Text = "設定値：";
            // 
            // txtSettingValue1
            // 
            this.txtSettingValue1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSettingValue1.Location = new System.Drawing.Point(72, 31);
            this.txtSettingValue1.Name = "txtSettingValue1";
            this.txtSettingValue1.Size = new System.Drawing.Size(323, 19);
            this.txtSettingValue1.TabIndex = 0;
            // 
            // SwapStageChildTextInput1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.txtSettingValue1);
            this.Controls.Add(this.lblTitle1);
            this.Name = "SwapStageChildTextInput1";
            this.Size = new System.Drawing.Size(417, 77);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle1;
        private System.Windows.Forms.TextBox txtSettingValue1;
    }
}
