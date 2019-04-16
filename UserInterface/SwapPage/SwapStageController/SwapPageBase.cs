//using Microsoft.Win32.SafeHandles;
using System.Windows.Forms;

namespace BAMSS.UI.SwapPage
{
    /// <summary>
    /// SwapPageControllerによってページ制御されるページの基底クラス
    /// </summary>
    public partial class SwapPageBase : UserControl, ISwapPage
    {
        /// <summary>
        /// ページ番号取得(0 Base)
        /// </summary>
        protected int _pageNo;
        /// <summary>
        /// 最初のページか否か
        /// </summary>
        protected bool _firstPage;
        /// <summary>
        /// 最後のページか否か
        /// </summary>
        protected bool _finalPage;
        /// <summary>
        /// 前ページへのトリガーコントロール
        /// </summary>
        protected Control _previousControl;
        /// <summary>
        /// 次ページへのトリガーコントロール
        /// </summary>
        protected Control _nextControl;
        /// <summary>
        /// 廃棄処理済確認
        /// </summary>
        protected bool _closed = false;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SwapPageBase()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 全工程終了時の処理
        /// </summary>
        /// <returns></returns>
        public virtual void OnFinishedAllProcesses() {  }
        /// <summary>
        /// 次へトリガー時の処理
        /// </summary>
        /// <returns></returns>
        public virtual int OnNext() { return 0; }
        /// <summary>
        /// 前へトリガー時の処理
        /// </summary>
        /// <returns></returns>
        public virtual int OnPrevious() { return 0; }
        /// <summary>
        /// ページ番号取得(0 Base)【外部からは読取専用】
        /// </summary>
        public virtual int PageNo { get { return this._pageNo; } protected set { this._pageNo = value; } }
        /// <summary>
        /// 廃棄処理済確認【読取専用】
        /// </summary>
        public virtual bool Closed { get { return this._closed; } }
        /// <summary>
        /// 最初のページかの取得【読取専用】
        /// </summary>
        public virtual bool FirstPage { get { return this._firstPage; } }
        /// <summary>
        /// 最後のページかの取得【読取専用】
        /// </summary>
        public virtual bool FinalPage { get { return this._finalPage; } }
        /// <summary>
        /// 前ページへのトリガーコントロール【読取専用】
        /// </summary>
        public virtual Control PreviousControl { get { return this._previousControl; } }
        /// <summary>
        /// 次ページへのトリガーコントロール【読取専用】
        /// </summary>
        public virtual Control NextControl { get { return this._nextControl; } }
        /// <summary>
        /// 廃棄処理
        /// </summary>
        public virtual void Close() { }
        /// <summary>
        /// ページ動作開始処理
        /// </summary>
        /// <param name="pageNo">ページ番号</param>
        /// <param name="firstPage">最初のページである場合true、そうでない場合false</param>
        /// <param name="finalPage">最後のページである場合true、そうでない場合false</param>
        /// <param name="previousControl">前ページへのトリガーコントロール</param>
        /// <param name="nextControl">次ページへのトリガーコントロール</param>
        public virtual void Action(int stageNo, bool firstPage, bool finalPage, Control previousControl, Control nextControl)
        {
            //this._pageNo = stageNo;
            this._firstPage = firstPage;
            this._finalPage = finalPage;
            this._previousControl = previousControl;
            this._nextControl = nextControl;
        }
        public virtual bool IsMyTurn() { return true; }
        public virtual bool IsAbleToSkipMe() { return true; }
        #region "IDisposable"
        //protected abstract void DisposeManaged();
        //protected abstract void DisposeUnmanaged();
        //bool disposed = false;              // Flag: Has Dispose already been called?
        //    //SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);      // Instantiate a SafeHandle instance.
        //    // Public implementation of Dispose pattern callable by consumers.
        //    public void Dispose()
        //    {
        //        Dispose(true);
        //        GC.SuppressFinalize(this);
        //    }
        //    // Protected implementation of Dispose pattern.
        //    protected virtual void Dispose(bool disposing)
        //    {
        //        if (disposed) return;
        //        if (disposing)
        //        {
        //            //handle.Dispose();
        //            // Free any other managed objects here.
        //        }
        //        // Free any unmanaged objects here.
        //        disposed = true;
        //    }
        #endregion
    }
}
