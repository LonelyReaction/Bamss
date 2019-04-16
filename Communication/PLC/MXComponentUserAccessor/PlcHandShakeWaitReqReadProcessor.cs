namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC通信受動H/S読取専用通信クラス（抽象クラス）
    /// </summary>
    /// <remarks></remarks>
    public abstract class PlcHandShakeWaitReqReadProcessor : PlcWatchProcessor
    {
        /// <summary>
        /// 応答フラグを立てるタイミング
        /// </summary>
        private RequestResponseActionTiming _responseOnTiming = RequestResponseActionTiming.Manual;
        /// <summary>
        /// 応答フラグを落とすタイミング
        /// </summary>
        private RequestResponseActionTiming _responseOffTiming = RequestResponseActionTiming.Manual;
        /// <summary>
        /// ハンドシェイクID
        /// </summary>
        protected string _handShakeID;
        /// <summary>
        /// ハンドシェイク情報
        /// </summary>
        protected PlcHandShakeInfo _handShakeInfo;
        /// <summary>
        /// 要求On時の処理
        /// </summary>
        protected abstract void RequestOn();
        /// <summary>
        /// 要求Off時の処理
        /// </summary>
        protected abstract void RequestOff();
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <param name="handShakeID">ハンドシェイクID</param>
        /// <param name="readAddrMapID">読込アドレスマップID</param>
        /// <param name="readItemMapID">読込項目マップID</param>
        /// <param name="confLoader">設定情報ローダー</param>
        /// <param name="responseOnTiming">レスポンスOnのタイミング</param>
        /// <param name="responseOffTiming">レスポンスOffのタイミング</param>
        public PlcHandShakeWaitReqReadProcessor(string connectionID, string handShakeID, string readAddrMapID, string readItemMapID, PlcConfigLoader confLoader, RequestResponseActionTiming responseOnTiming, RequestResponseActionTiming responseOffTiming)
            : base(connectionID, readAddrMapID, readItemMapID, confLoader)
        {
            this._handShakeInfo = confLoader.LoadPLCHandShakeInfo(handShakeID);
            this._responseOnTiming = responseOnTiming;
            this._responseOffTiming = responseOffTiming;
        }
        /// <summary>
        /// 定周期監視処理（インスタンス生成元のタイマから起動してもらう）
        /// </summary>
        /// <remarks></remarks>
        public override void WatchEvent()
        {
            try { this.ProcessEveryTime(); }
            catch { throw; }
            try
            {   //ハンドシェイク処理
                var request = new int[1];
                var response = new int[1];
                if ((!this._plcAccessor.ReadWord(this._handShakeInfo.RequestAddress,ref request)) || (!this._plcAccessor.ReadWord(this._handShakeInfo.ResponseAddress,ref response))) { return; }
                if ((request[0] != 0) & (response[0] == 0)) 
                {
                    if (this._responseOnTiming == RequestResponseActionTiming.Before) this.ResponseToPLC(1);
                    this.RequestOn();
                    if (this._responseOnTiming == RequestResponseActionTiming.After) this.ResponseToPLC(1);
                }
                else if ((request[0] == 0) & (response[0] != 0))
                {
                    if (this._responseOffTiming == RequestResponseActionTiming.Before) this.ResponseToPLC(0);
                    this.RequestOff();
                    if (this._responseOffTiming == RequestResponseActionTiming.After) this.ResponseToPLC(0);
                }
                else { return; }
            }
            catch { throw; }
        }
        /// <summary>
        /// 毎回する処理
        /// </summary>
        /// <remarks></remarks>
        protected virtual void ProcessEveryTime() { }
        /// <summary>
        /// PLCへの応答コード書込み
        /// </summary>
        /// <param name="responseCode">応答値</param>
        /// <remarks></remarks>
        protected void ResponseToPLC(int responseCode) { this._plcAccessor.WriteWord(this._handShakeInfo.ResponseAddress, new int[] { responseCode }); }
    }
}
