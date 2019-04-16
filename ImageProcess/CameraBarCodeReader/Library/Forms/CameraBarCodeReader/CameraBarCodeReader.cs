using DirectShowLib;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace BAMSS.Forms.Control
{
    public delegate void ChangeDisplayReadingStatusDelegate(bool onReading);
    public delegate void BarcodeReadEventHandler(string code);
    public delegate void CameraChangingEventHandler(ref CameraChangArgs e);
    public delegate void CameraChangedEventHandler(CameraChangArgs e);
    /// <summary>
    /// カメラデバイスによるバーコード読み取りコントロール
    /// </summary>
    public partial class CameraBarCodeReader : UserControl
    {
        #region "Private Static Methods"
            /// <summary>
            /// 使用可能なカメラの一覧情報取得
            /// </summary>
            /// <returns></returns>
            private static Dictionary<int, string> _GetCameraList()
            {
                var cameraList = new Dictionary<int, string>();
                DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList().ForEach(c => cameraList.Add(cameraList.Count, c.Name));
                return cameraList;
            }
            /// <summary>
            /// 画像の補正機能【現在非推奨：逆に認識しにくくなる】
            /// </summary>
            /// <param name="img">補正対象ビットマップ</param>
            /// <returns></returns>
            private static Bitmap _CorrectionBitmap(Bitmap img)
            {
                int width = img.Width;
                int height = img.Height;
                var dstImg = new Bitmap(width, height);
                var bmpread = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                try
                {
                    var bmpwrite = dstImg.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    try
                    {
                        int blackness = 255;
                        int whiteness = -1;
                        for (int i = 0; i < (width * height * 3); )
                        {
                            var r = Marshal.ReadByte(bmpread.Scan0, i++);
                            var g = Marshal.ReadByte(bmpread.Scan0, i++);
                            var b = Marshal.ReadByte(bmpread.Scan0, i++);
                            int ave = (int)new List<int> { r, g, b }.Average();
                            // 最も暗い色を抜き出して「黒」を定義
                            blackness = Math.Min(blackness, ave);
                            // 128以上の値を平均して「白」を定義
                            if (ave >= 128)
                            {
                                if (whiteness == -1) whiteness = ave;
                                else whiteness = (whiteness + ave) / 2;
                            }
                        }
                        var step = 255d / ((double)(whiteness - blackness));
                        for (int i = 0, j = 0; i < (width * height * 3); )
                        {
                            var r = Marshal.ReadByte(bmpread.Scan0, i++);
                            var g = Marshal.ReadByte(bmpread.Scan0, i++);
                            var b = Marshal.ReadByte(bmpread.Scan0, i++);
                            var v = new List<int> { r, g, b }.Average();
                            v -= blackness;
                            v *= step;
                            v = Math.Min(255, Math.Max(0, v));
                            Marshal.WriteByte(bmpwrite.Scan0, j++, (byte)v);
                            Marshal.WriteByte(bmpwrite.Scan0, j++, (byte)v);
                            Marshal.WriteByte(bmpwrite.Scan0, j++, (byte)v);
                        }
                        // 暗さの調整
                        int darkness = 255 - whiteness;
                        whiteness -= darkness;
                        // 明るさの調整
                        blackness += blackness / 2;
                        // 黒と白の数値の差を取り閾値を作る
                        int adjust = (whiteness - blackness) / 3;
                        blackness += adjust;
                        whiteness -= adjust;
                        int diff = whiteness - blackness;
                        for (int i = 0, j = 0; i < (width * height * 3); )
                        {
                            var r = Marshal.ReadByte(bmpread.Scan0, i++);
                            var g = Marshal.ReadByte(bmpread.Scan0, i++);
                            var b = Marshal.ReadByte(bmpread.Scan0, i++);
                            // 黒の閾値以下ならそのピクセルは黒
                            if (r < blackness || g < blackness || b < blackness)
                            {
                                Marshal.WriteByte(bmpwrite.Scan0, j++, 0);
                                Marshal.WriteByte(bmpwrite.Scan0, j++, 0);
                                Marshal.WriteByte(bmpwrite.Scan0, j++, 0);
                            }
                            // 白の閾値以上ならそのピクセルは白
                            else if (r > whiteness || g > whiteness || b > whiteness)
                            {
                                Marshal.WriteByte(bmpwrite.Scan0, j++, 255);
                                Marshal.WriteByte(bmpwrite.Scan0, j++, 255);
                                Marshal.WriteByte(bmpwrite.Scan0, j++, 255);
                            }
                            else
                            {
                                double v = new List<int> { r, g, b }.Average();
                                v -= diff;
                                v *= 3;
                                v = Math.Min(255, Math.Max(0, v));
                                Marshal.WriteByte(bmpwrite.Scan0, j++, (byte)v);
                                Marshal.WriteByte(bmpwrite.Scan0, j++, (byte)v);
                                Marshal.WriteByte(bmpwrite.Scan0, j++, (byte)v);
                            }
                        }
                        return dstImg;
                    }
                    catch { }
                    finally { dstImg.UnlockBits(bmpwrite); }
                }
                catch { }
                finally { img.UnlockBits(bmpread); }
                return img;
            }
        #endregion
        #region "Private Instance Field Members"
            private Dictionary<int, string> _cameraList = CameraBarCodeReader._GetCameraList();
            private ContextMenuStrip _cmsSubMenu = new ContextMenuStrip();
            private VideoCapture _videoCap;
            private Mat _mat;
            private Bitmap _snapShot;
            private object _lockImage = new object();
            private bool _onCapturing = false;
            private bool _onReading = false;
            private bool _doubleRead = true;
            private bool _correction = false;
            private int _cameraNo;
            private long _readScanTime = 5000;
            private RotateFlipType _RotateType = RotateFlipType.RotateNoneFlipNone;
        #endregion
        #region "Public Instance Events and Delegates"
            /// <summary>
            /// カメラ番号が変更される前に発生するイベント
            /// </summary>
            public event CameraChangingEventHandler OnCameraChanging = delegate { };
            /// <summary>
            /// カメラ番号が変更された後に発生するイベント
            /// </summary>
            public event CameraChangedEventHandler OnCameraChanged = delegate { };
            /// <summary>
            /// バーコード読み取り時に発生するイベント
            /// </summary>
            public event BarcodeReadEventHandler OnBarcodeRead = delegate { };
            /// <summary>
            /// バーコード読取中状態による画面コントロールの操作処理
            /// </summary>
            private ChangeDisplayReadingStatusDelegate ChangeDisplayReadingStatus;
        #endregion

        #region "Public Instance Propertys"
            [Browsable(false)]
            public bool OnCapturing
            {
                get { return this._onCapturing; }
            }
            [Browsable(false)]
            public bool OnReading
            {
                get { return this._onReading; }
            }
            [Browsable(true)]
            [Description("バーコード読み取りを行う最大試行時間を指定します。（デフォルト:5000ms）")]
            [Category("デバイス")]
            public long ReadScanTime
            {
                get { return this._readScanTime; }
                set { this._readScanTime = value; }
            }
            [Browsable(true)]
            [Description("使用するカメラデバイスの番号を指定します。")]
            [Category("デバイス")]
            public int CameraNo
            {
                get { return this._cameraNo; }
                set
                {
                    if (this.DesignMode)
                    {
                        this._cameraNo = value;
                    }
                    else 
                    {
                        if (this._cameraNo != value)
                        {
                            var beforeNo = this._cameraNo;
                            var args = new CameraChangArgs(beforeNo, value, false);
                            this.OnCameraChanging(ref args);
                            if (args.Cancel) return;
                            this._cameraNo = value;
                            this.OnCameraChanged(args);
                        }
                    }
                }
            }
            [Browsable(true)]
            [Description("バーコードを2二回読み取って同値チェックを行うかの指定。")]
            [Category("デバイス")]
            public bool DoubleRead
            {
                get { return this._doubleRead; }
                set { this._doubleRead = value; }
            }
            [Browsable(true)]
            [Description("バーコード解析画像を補正するかの指定。(現在、非推奨機能)")]
            [Category("デバイス")]
            public bool Correction
            {
                get { return this._correction; }
                set { this._correction = value; }
            }
            [Browsable(true)]
            [Description("イメージの配置とコントロールの変更をどのように扱うか制御します。（表示方法のみの制御であり、バーコードの読み取りには影響しません）")]
            [Category("デバイス")]
            public PictureBoxSizeMode SizeMode
            {
                get { return this.pbxCapture.SizeMode; }
                set { this.pbxCapture.SizeMode = value; }
            }
            public RotateFlipType RotateType
            {
                get { return this._RotateType; }
                set { this._RotateType = value; }
            }
        #endregion
        #region "Public Instance Constructor"
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CameraBarCodeReader()
            {
                InitializeComponent();
                if (!this.DesignMode)
                {
                    this.ChangeDisplayReadingStatus = new ChangeDisplayReadingStatusDelegate(SetDisplayReadingStatus);
                    this.pbxCapture.ContextMenuStrip = this._cmsSubMenu;
                    this.pbxCapture.SizeMode = PictureBoxSizeMode.StretchImage;
                    var tsmSelectCamera = new ToolStripMenuItem("Select Camera");
                    var tsmStartRead = new ToolStripMenuItem("Read Barcode");
                    this._cmsSubMenu.Items.Add(tsmSelectCamera);
                    this._cmsSubMenu.Items.Add(tsmStartRead);
                    this._cameraList.ToList().ForEach(c =>
                    {
                        var mi = tsmSelectCamera.DropDownItems.Add(c.Value);
                        mi.Tag = tsmSelectCamera.DropDownItems.Count - 1;
                    });
                    tsmSelectCamera.DropDownItemClicked += tsmSelectCamera_DropDownItemClicked;
                    tsmStartRead.Click += tsmStartRead_Click;
                }
            }
        #endregion
        #region "Public Instance Methods"
            /// <summary>
            /// 使用可能なカメラの一覧情報を取得
            /// </summary>
            /// <returns></returns>
            public Dictionary<int, string> GetCameraList() { return this._cameraList; }
            /// <summary>
            /// カメラ動作停止
            /// </summary>
            public void StopCapture()
            {
                if (!this._onCapturing) return;
                this._onCapturing = false;
            }
            /// <summary>
            /// カメラ動作開始
            /// </summary>
            public void StartCapture()
            {
                if (this._onCapturing) return;
                this.ReadyToCameraForce();
                this._onCapturing = true;
                this.CaptureAsync();
            }
            /// <summary>
            /// スナップショット画像取得
            /// </summary>
            public void TakeSnapShot()
            {
                lock (this._lockImage)
                {
                    if (this._snapShot != null) this._snapShot.Dispose();
                    this._snapShot = (this._correction ? CameraBarCodeReader._CorrectionBitmap((Bitmap)this.pbxCapture.Image.Clone()) : (Bitmap)this.pbxCapture.Image.Clone());
                }
            }
            /// <summary>
            /// スナップショット画像保存(保存形式=デフォルトpng)
            /// </summary>
            /// <param name="path">保存先ファイルパス</param>
            public void SaveSnapShot(string path) { this.SaveSnapShot(path, ImageFormat.Png); }
            /// <summary>
            /// スナップショット画像保存(保存形式指定)
            /// </summary>
            /// <param name="path">保存先ファイルパス</param>
            /// <param name="format">保存形式</param>
            public void SaveSnapShot(string path, ImageFormat format) { lock (this._lockImage) this._snapShot.Save(path, format); }
            /// <summary>
            /// バーコード読み取り処理 ≪Asynchronous Support≫
            /// </summary>
            /// <returns></returns>
            public async Task<string> ReadAsync()
            {
                string result = null;
                if (!this._onReading)
                {
                    try
                    {
                        this.Invoke(ChangeDisplayReadingStatus, new object[] { true });
                        this._onReading = true;
                        var timeLimit = DateTime.Now.AddMilliseconds(this._readScanTime);
                        var barcodeReader = new BarcodeReader();
                        barcodeReader.AutoRotate = true;
                        barcodeReader.TryInverted = true;
                        result = await Task.Run(() =>
                        {
                            Bitmap bmp = null;
                            string checkData = null;
                            while (DateTime.Now < timeLimit)
                            {
                                try
                                {
                                    this.ReadyToCameraIfNeed();
                                    lock (this._lockImage)
                                    {
                                        this._videoCap.Read(this._mat);
                                        bmp = (Bitmap)(this._mat.ToBitmap().Clone());
                                    }
                                    var readResult = barcodeReader.Decode((this._correction ? CameraBarCodeReader._CorrectionBitmap(bmp) : bmp));
                                    if ((readResult != null) && (readResult.Text != null))
                                    {
                                        if ((!this._doubleRead) || ((checkData != null) && (checkData == readResult.Text))) return readResult.Text;
                                        else checkData = readResult.Text;
                                    }
                                }
                                catch { this.ReadyToCameraForce(); }
                                finally { if (bmp != null) bmp.Dispose(); }
                            }
                            throw new TimeoutException("バーコード読み取りが指定時間内に成功しませんでした。");
                        }).ConfigureAwait(false);
                    }
                    catch { throw; }
                    finally
                    {
                        this.Invoke(ChangeDisplayReadingStatus, new object[] { false });
                        this._onReading = false;
                        this.DisposeCamera();
                    }
                }
                return result;
            }
        #endregion
        #region "Private Instance Methods"
            /// <summary>
            /// カメラ廃棄処理
            /// </summary>
            private void DisposeCamera()
            {
                if ((this._onCapturing) || (this._onReading)) return;
                lock (this._lockImage)
                {
                    this._mat.Dispose();
                    this._mat = null;
                    this._videoCap.Dispose();
                    this._videoCap = null;
                }
            }
            /// <summary>
            /// カメラ使用準備（使用中であれば一旦廃棄してから生成）
            /// </summary>
            private void ReadyToCameraForce()
            {
                lock (this._lockImage)
                {
                    if (this._videoCap != null) this._videoCap.Dispose();
                    if (this._mat != null) this._mat.Dispose();
                    this._videoCap = new VideoCapture();
                    this._mat = new Mat();
                    this._videoCap.Open(this._cameraNo);
                }
            }
            /// <summary>
            /// カメラ使用準備（カメラが使用中でない場合のみ生成）
            /// </summary>
            private void ReadyToCameraIfNeed()
            {
                lock (this._lockImage)
                {
                    if (this._mat == null) this._mat = new Mat();
                    if (this._videoCap == null)
                    {
                        this._videoCap = new VideoCapture();
                        this._videoCap.Open(this._cameraNo);
                    }
                }
            }
            /// <summary>
            /// 右クリックメニュー（バーコード読取）
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private async void tsmStartRead_Click(object sender, EventArgs e)
            {
                try
                {
                    this.OnBarcodeRead(await this.ReadAsync());
                }
                catch (TimeoutException ex)
                {
                    MessageBox.Show(this, ex.Message, "読取失敗", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            /// <summary>
            /// バーコード読込中／アイドル中状態による画面コントロールの制御処理
            /// </summary>
            /// <param name="onReading"></param>
            private void SetDisplayReadingStatus(bool onReading)
            {
                this.pbxCapture.Cursor = onReading ? Cursors.WaitCursor : Cursors.Default;
            }
            /// <summary>
            /// 右クリックメニュー（カメラ選択）ハンドラ
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tsmSelectCamera_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {
                var changeIntoNo = int.Parse(e.ClickedItem.Tag.ToString());
                if (changeIntoNo != this.CameraNo) 
                {
                    var beforeNo = this._cameraNo;
                    var args = new CameraChangArgs(beforeNo, changeIntoNo, true);
                    this.OnCameraChanging(ref args);
                    if (args.Cancel) return;
                    this._cameraNo = changeIntoNo;
                    this.OnCameraChanged(args);
                    if (this._onCapturing)
                    {
                        this.StopCapture();
                        this.StartCapture();
                    }
                    else if ((this._videoCap != null) || (this._mat != null))
                    {
                        this.ReadyToCameraForce();
                    }
                }
            }
            /// <summary>
            /// 画像キャプチャ処理
            /// </summary>
            private async void CaptureAsync()
            {
                while (this._onCapturing)
                {
                    try
                    {
                        await Task.Run(() => 
                        {
                            this.ReadyToCameraIfNeed();
                            lock (this._lockImage) this._videoCap.Read(this._mat);
                        }).ConfigureAwait(true);
                        if (this.pbxCapture.Image != null) this.pbxCapture.Image.Dispose();
                        //this.pbxCapture.Image = this._mat.ToBitmap();
                        var bitMap = this._mat.ToBitmap();
                        bitMap.RotateFlip(this._RotateType);
                        this.pbxCapture.Image = bitMap;
                    }
                    catch { /* Ignore Exceptions. */ }
                }
                this.DisposeCamera();
            }
        #endregion
    }
    /// <summary>
    /// CameraBarCodeReaderクラスのカメラ番号変更関連イベント(OnCameraChanging/OnCameraChanged)のイベント情報クラス
    /// </summary>
    public class CameraChangArgs : EventArgs
    {
        /// <summary>
        /// 変更前のカメラ番号（読み取り専用）
        /// </summary>
        public int BeforeNo { get; private set; }
        /// <summary>
        /// 適用されたカメラ番号（読み取り専用）
        /// </summary>
        public int ChangedNo { get; private set; }
        /// <summary>
        /// 変更がメニューオペレーションによって行われたかを示す真偽値（読み取り専用）
        /// </summary>
        public bool ChangedByMenu { get; private set; }
        /// <summary>
        /// 変更の拒否（イベントハンドラ側にて指定）
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="beforeNo">変更前のカメラ番号</param>
        /// <param name="changedNo">適用されたカメラ番号</param>
        /// <param name="changedByMenu">変更がメニューオペレーションによって行われたかを示す真偽値</param>
        public CameraChangArgs(int beforeNo, int changedNo, bool changedByMenu)
        {
            this.BeforeNo = beforeNo;
            this.ChangedNo = changedNo;
            this.ChangedByMenu = changedByMenu;
            this.Cancel = false;
        }
    }
}
