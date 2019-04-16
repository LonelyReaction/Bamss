using System.Windows.Forms;

namespace BAMSS.UI.SwapPage
{
    /// <summary>
    /// 終了表示ページコントロール（遷移ボタンを非表示にして、単一メッセージのみ表示します）
    /// </summary>
    public partial class SwapPageGeneralCreditsRoll : SwapPageBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">終了メッセージ</param>
        public SwapPageGeneralCreditsRoll(string message)
        {
            InitializeComponent();
            this.lblMessage.Text = message;
        }
        /// <summary>
        /// 動作開始処理
        /// </summary>
        /// <param name="pageNo">ページ番号</param>
        /// <param name="firstPage">最初のページである場合true、そうでない場合false</param>
        /// <param name="finalPage">最後のページである場合true、そうでない場合false</param>
        /// <param name="previousControl">前ページへのトリガーコントロール</param>
        /// <param name="nextControl">次ページへのトリガーコントロール</param>
        public override void Action(int pageNo, bool firstPage, bool finalPage, Control previousControl, Control nextControl)
        {
            base.Action(pageNo, firstPage, finalPage, previousControl, nextControl);
            this._nextControl.Visible = false;
            this._previousControl.Visible = false;
        }
    }
}
