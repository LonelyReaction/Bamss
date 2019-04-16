using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using BAMSS.Extensions;

namespace BAMSS.UI.SwapPage
{
    /// <summary>
    /// ページ制御クラス
    /// </summary>
    public partial class SwapPageController : UserControl
    {
        /// <summary>
        /// 既定の終了メッセージ
        /// </summary>
        private const string _defaultMessageOnFinished = "お疲れ様でした、作業は完了しました。";
        /// <summary>
        /// ページオブジェクトのリスト
        /// </summary>
        private List<SwapPageBase> _pages = new List<SwapPageBase>();
        /// <summary>
        /// 全工程が終了しているか否か
        /// </summary>
        private bool _finishedAllProcesses;
        /// <summary>
        /// 現在表示中のページ番号(0ベース)
        /// </summary>
        private int _nowOnPageNo;
        /// <summary>
        /// 終了時に表示するメッセージ
        /// </summary>
        private string _messageOnFinished = _defaultMessageOnFinished;
        private Button _nextButton;
        private Button _prevButton;
        public delegate void OnErrorEventHandler(Exception ex);
        public event OnErrorEventHandler OnError;
        public delegate void QueryPageLoadEventHaandler(SwapPageBase fromPage, SwapPageBase toPage, ref bool Canceled);
        public event QueryPageLoadEventHaandler QueryNextPageLoad;
        public event QueryPageLoadEventHaandler QueryPrevPageLoad;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SwapPageController()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ページのリスト
        /// </summary>
        public List<SwapPageBase> Pages { get { return this._pages; } }
        /// <summary>
        /// ページ制御開始処理
        /// </summary>
        /// <param name="pageNo">0ベースのページ番号、
        /// 最初からの場合は0、途中再開の場合は、再開起点となるページ番号、
        /// 作業が終了している場合は、int.MaxValue等の最大ページ番号より大きな数を指定して下さい。</param>
        public void Start(int pageNo = 0, string messageOnFinished = _defaultMessageOnFinished)
        {
            if (this._nextButton == null) throw new NullReferenceException("次ステップへ遷移する為のボタンが割りついていません。");
            if (this._prevButton == null) throw new NullReferenceException("前ステップへ遷移する為のボタンが割りついていません。");
            this._nextButton.Click -= btnNextStep_Click;
            this._prevButton.Click -= btnPrevStep_Click;
            this._nextButton.Click += btnNextStep_Click;
            this._prevButton.Click += btnPrevStep_Click;
            //if (pageNo >= this._pages.Count) throw new InvalidOperationException("指定されたページ番号が有効なページ番号として認識できませんでした。");
            if ((this._pages == null) || (this._pages.Count < 1)) throw new InvalidOperationException("ページコントロールが登録されていない為、画面制御を開始できませんでした。");
            this._messageOnFinished = messageOnFinished;
            //MessageBox.Show("_finishedAllProcessesの復元方法はもう少し考えなさい。_finishedAllProcesses処理でエラーが発生した場合_finishedAllProcesses処理の結果もどこかに覚えておかないと各ページのDB更新だけでは解らない。→各ページ毎に_finishedAllProcesses処理の結果を持つ方が良いな。");
            this._finishedAllProcesses = (pageNo >= this._pages.Count);
            this.LoadPage(pageNo);
        }
        /// <summary>
        /// ページをロードする処理（ページを読み込んで、ページ処理を開始します）
        /// </summary>
        /// <param name="pageNo">開始するページ番号(0ベースでページ番号を指定、最初からの場合0で途中再開可能。)</param>
        private void LoadPage(int pageNo, bool recursiveCall = false)
        {
            this.ClearPages();
            var maxPageNo = this._pages.Where(c => (c.IsMyTurn())).Max(c => c.PageNo);
            if (pageNo > maxPageNo)
            {
                if (!this._finishedAllProcesses) 
                {   //全工程完了タイミングでの各ページ処理をコール
                    foreach (SwapPageBase ctrl in this._pages) { ctrl.OnFinishedAllProcesses(); }
                    this._finishedAllProcesses = true;
                }
                this._nextButton.Visible = false;
                this._prevButton.Visible = false;
                SwapPageBase page = new SwapPageGeneralCreditsRoll(this._messageOnFinished);
                page.Dock = DockStyle.Fill;
                this.pnlStage.Controls.Add(page);
                this._nowOnPageNo = this._pages.Count;
            }
            else
            {
                try
                {
                    SwapPageBase caughtPage = null;
                    SwapPageBase nextPage = null;
                    SwapPageBase nearestPrevPage = this._pages.Where(c => (c.PageNo < pageNo)).OrderByDescending(c => c.PageNo).Where(c => (c.IsMyTurn())).FirstOrDefault();
                    if (pageNo < this._nowOnPageNo)
                    {   //On Previous
                        nextPage = this._pages.Where(c => (c.PageNo <= pageNo)).OrderByDescending(c => c.PageNo).Where(c => (c.IsMyTurn())).FirstOrDefault();
                    }
                    else if ((pageNo > this._nowOnPageNo) || (pageNo == 0))
                    {   //On Next
                        nextPage = this._pages.Where(c => (c.PageNo >= pageNo)).OrderBy(c => c.PageNo).Where(c => (c.IsMyTurn())).FirstOrDefault();
                        if (nextPage != null)
                        {
                            caughtPage = this._pages.Where(c => ((c.PageNo >= pageNo) && (c.PageNo < nextPage.PageNo))).OrderBy(c => c.PageNo).Where(c => (!c.IsAbleToSkipMe())).FirstOrDefault();
                        }
                    }
                    if (caughtPage != null)
                    {
                        throw new SwapPageControlException(caughtPage, string.Format("飛び越せないページがあります。(Page No:[{0}])", caughtPage.PageNo + 1));
                    }
                    if (nextPage == null)
                    {   //On Error or Recursive Call from Catch exception sections then Keep processing page.
                        nextPage = this._pages[this._nowOnPageNo];
                    }
                    this._nextButton.Visible = true;
                    this._nextButton.Enabled = true;
                    this._prevButton.Visible = true;
                    this._prevButton.Enabled = (nearestPrevPage != null); //(nextPage.PageNo > 0);
                    nextPage.Dock = DockStyle.Fill;
                    nextPage.Action(nextPage.PageNo, (nextPage.PageNo < 1), (nextPage.PageNo >= maxPageNo), this._prevButton, this._nextButton);
                    this.pnlStage.Controls.Add(nextPage);
                    nextPage.Focus();
                    this._nowOnPageNo = nextPage.PageNo;
                }
                catch (SwapPageControlException ex)
                {   //未完了の事前作業が次工程の前にあって進めない場合
                    this.OnError(ex);
                    throw;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(string.Format("{0}\r\n(Recursive:[{1}])", ex.Message, recursiveCall ? "true" : "false"), "ページ初期化エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //if ((!recursiveCall) || (pageNo > 0)) this.LoadPage(this._nowOnPageNo, true);
                    //else throw;
                    this.OnError(ex);
                    if (pageNo == this._nowOnPageNo) throw;
                    if ((!recursiveCall) || (pageNo > 0)) this.LoadPage(this._nowOnPageNo, true);
                }
            }
        }
        public string PagePositionForDisplay 
        { 
            get 
            {
                try
                {
                    return string.Format("{0}/{1}", (this._nowOnPageNo + 1), this._pages.Count);
                }
                catch
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 前ページボタンのページ遷移イベント処理
        /// </summary>
        /// <param name="sender">イベント対象のコントロール</param>
        /// <param name="e">イベントパラメータ</param>
        private void btnPrevStep_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this._pages[this._nowOnPageNo].Closed)
                {
                    var canceled = false;
                    int nextPageNo = this.GetPageNoFromRelativePos(this._pages[this._nowOnPageNo].OnPrevious());
                    if (nextPageNo != int.MaxValue)
                    {
                        this.QueryPrevPageLoad(this._pages[this._nowOnPageNo], this._pages[nextPageNo], ref canceled);
                    }
                    if ((!canceled) && (nextPageNo != this._nowOnPageNo)) this.LoadPage(nextPageNo);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(this, ex.Message, "ページ移動異常(前ページへの移動)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// 次ページボタン等のページ遷移イベント処理
        /// </summary>
        /// <param name="sender">イベント対象のコントロール</param>
        /// <param name="e">イベントパラメータ</param>
        private void btnNextStep_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this._pages[this._nowOnPageNo].Closed)
                {
                    var canceled = false;
                    int nextPageNo = this.GetPageNoFromRelativePos(this._pages[this._nowOnPageNo].OnNext());
                    if (nextPageNo != int.MaxValue)
                    {
                        this.QueryNextPageLoad(this._pages[this._nowOnPageNo], this._pages[nextPageNo], ref canceled);
                    }
                    if ((!canceled) && (nextPageNo != this._nowOnPageNo)) this.LoadPage(nextPageNo);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(this, ex.Message, "ページ移動異常(次ページへの移動)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// ページ制御コントロール上のページコントロールをクリアします。
        /// </summary>
        private void ClearPages() 
        { 
            this.pnlStage.Controls.Clear();
        }
        /// <summary>
        /// ページ制御コントロール上のページコントロールの廃棄処理を呼び出します。
        /// </summary>
        private void DisposePages() { foreach (SwapPageBase ctrl in this.pnlStage.Controls) { ctrl.Close(); } }
        /// <summary>
        /// 現在ページからの指定オフセットによるページ番号取得
        /// </summary>
        /// <param name="relativePos">相対位置情報(現在ページから+-ページオフセット値)</param>
        /// <returns>オフセットを加味したページ番号。
        /// 最終ページ番号よりも大きくなる場合はint.MaxValueを返します、この場合作業が完了している事を意味します。</returns>
        private int GetPageNoFromRelativePos(int relativePos)
        {
            if (relativePos == 0) return this._nowOnPageNo;
            if ((this._nowOnPageNo + relativePos) < 0) return 0;
            if ((this._nowOnPageNo + relativePos) >= this._pages.Count) return int.MaxValue;
            return (this._nowOnPageNo + relativePos);
        }
        [Browsable(true)]
        [Description("次ステップへ移行するボタンを割り付けます、\r\nこのボタンを割り付けないとページ遷移操作ができません。")]
        public Button NextButton 
        { 
            get 
            {
                return this._nextButton;
            }
            set
            {
                this._nextButton = value;
            }
        }
        [Browsable(true)]
        [Description("前ステップへ移行するボタンを割り付けます、\r\nこのボタンを割り付けないとページ遷移操作ができません。")]
        public Button PreviousButton 
        { 
            get 
            {
                return this._prevButton; 
            }
            set
            {
                this._prevButton = value;
            }
        }
    }
}
