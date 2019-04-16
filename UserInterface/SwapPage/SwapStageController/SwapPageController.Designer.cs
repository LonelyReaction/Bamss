namespace BAMSS.UI.SwapPage
{
    partial class SwapPageController
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
            this.DisposePages();
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlStage = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlStage
            // 
            this.pnlStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStage.Location = new System.Drawing.Point(0, 0);
            this.pnlStage.Name = "pnlStage";
            this.pnlStage.Size = new System.Drawing.Size(416, 265);
            this.pnlStage.TabIndex = 2;
            // 
            // SwapPageController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlStage);
            this.MinimumSize = new System.Drawing.Size(212, 140);
            this.Name = "SwapPageController";
            this.Size = new System.Drawing.Size(416, 265);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlStage;
    }
}
