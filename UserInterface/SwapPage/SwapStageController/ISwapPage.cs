using System.Windows.Forms;

namespace BAMSS.UI.SwapPage
{
    /// <summary>
    /// SwapPageControllerにて扱われるSwapPageBaseオブジェクトに必要なインターフェイス
    /// </summary>
    public interface ISwapPage
    {
        /// <summary>
        /// ページ番号取得(0 Base)【読取専用】
        /// </summary>
        int PageNo { get; }
        /// <summary>
        /// 廃棄処理済確認【読取専用】
        /// </summary>
        bool Closed { get; }
        /// <summary>
        /// 最初のページかの取得【読取専用】
        /// </summary>
        bool FirstPage { get; }
        /// <summary>
        /// 最後のページかの取得【読取専用】
        /// </summary>
        bool FinalPage { get; }
        /// <summary>
        /// 前ページへのトリガーコントロール【読取専用】
        /// </summary>
        Control PreviousControl { get; }
        /// <summary>
        /// 次ページへのトリガーコントロール【読取専用】
        /// </summary>
        Control NextControl { get; }
        /// <summary>
        /// ページ動作開始処理
        /// </summary>
        /// <param name="pageNo">ページ番号</param>
        /// <param name="firstPage">最初のページである場合true、そうでない場合false</param>
        /// <param name="finalPage">最後のページである場合true、そうでない場合false</param>
        /// <param name="previousControl">前ページへのトリガーコントロール</param>
        /// <param name="nextControl">次ページへのトリガーコントロール</param>
        void Action(int pageNo, bool firstPage, bool finalPage, Control previousControl, Control nextControl);
        /// <summary>
        /// 次へトリガー時の処理
        /// </summary>
        /// <returns></returns>
        int OnNext();
        /// <summary>
        /// 前へトリガー時の処理
        /// </summary>
        /// <returns></returns>
        int OnPrevious();
        /// <summary>
        /// 全工程終了時の処理
        /// </summary>
        void OnFinishedAllProcesses();
        /// <summary>
        /// 廃棄処理
        /// </summary>
        void Close();
    }
}
