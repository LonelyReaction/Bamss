namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC通信能動H/S読取専用通信クラス（抽象クラス）
    /// </summary>
    /// <remarks></remarks>
    public abstract class PlcHandShakeSendReqReadProcessor : PlcWatchProcessor
    {
        /// <summary>
        /// 要求フラグを落とすタイミング
        /// </summary>
        private RequestResponseActionTiming _requestOffTiming = RequestResponseActionTiming.Manual;
        /// <summary>
        /// ハンドシェイクID
        /// </summary>
        protected string _handShakeID;
        /// <summary>
        /// ハンドシェイク情報
        /// </summary>
        protected PlcHandShakeInfo _handShakeInfo;
        /// <summary>
        /// 要求On時の処理(通信開始トリガー処理)
        /// </summary>
        protected virtual void RequestOn() { this.RequestToPLC(1); }
        /// <summary>
        /// 応答On時の処理
        /// </summary>
        protected abstract void ResponseOn();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <param name="handShakeID">ハンドシェイクID</param>
        /// <param name="readMapID">読込マップID</param>
        /// <param name="confLoader">設定情報ローダー</param>
        /// <param name="requestOffTiming">要求フラグを落とすタイミング</param>
        public PlcHandShakeSendReqReadProcessor(string connectionID, string handShakeID, string readAddrMapID, string readItemMapID, PlcConfigLoader confLoader, RequestResponseActionTiming requestOffTiming)
            : base(connectionID, readAddrMapID, readItemMapID, confLoader)
        {
            this._handShakeInfo = confLoader.LoadPLCHandShakeInfo(handShakeID);
            this._requestOffTiming = requestOffTiming;
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
                if ((!this._plcAccessor.ReadWord(this._handShakeInfo.RequestAddress, ref request)) || (!this._plcAccessor.ReadWord(this._handShakeInfo.ResponseAddress, ref response))) return;
                if ((request[0] != 0) & (response[0] != 0))     //応答着信処理
                {
                    if (this._requestOffTiming == RequestResponseActionTiming.Before) this.RequestToPLC(0);
                    this.ResponseOn();
                    if (this._requestOffTiming == RequestResponseActionTiming.After) this.RequestToPLC(0);
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
        /// PLCへの要求コード書込み
        /// </summary>
        /// <param name="requestCode">要求値</param>
        /// <remarks></remarks>
        protected void RequestToPLC(int requestCode) 
        {
            this._plcAccessor.WriteWord(this._handShakeInfo.ResponseAddress, new int[] { 0 });
            this._plcAccessor.WriteWord(this._handShakeInfo.RequestAddress, new int[] { requestCode });
        }
    }
}
