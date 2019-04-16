namespace BAMSS.MXComponent
{
    /// <summary>
    /// ハンドシェイク情報クラス
    /// </summary>
    public class PlcHandShakeInfo
    {
        private string _requestAddress;
        private string _responseAddress;
        private int _scanTime;
        /// <summary>
        /// PLCハンドシェイク情報クラス
        /// </summary>
        /// <param name="requestAddress">要求ビットアドレス</param>
        /// <param name="responseAddress">応答ビットアドレス</param>
        /// <param name="scanTime">現在未使用</param>
        public PlcHandShakeInfo(string requestAddress, string responseAddress, int scanTime)
        {
            this._requestAddress = requestAddress;
            this._responseAddress = responseAddress;
            this._scanTime = scanTime;
        }
        /// <summary>
        /// 要求ビットアドレス
        /// </summary>
        public string RequestAddress
        {
            get { return this._requestAddress; }
        }
        /// <summary>
        /// 応答ビットアドレス
        /// </summary>
        public string ResponseAddress
        {
            get { return this._responseAddress; }
        }
        /// <summary>
        /// スキャン間隔【現在未使用】
        /// </summary>
        public int ScanTime
        {
            get { return this._scanTime; }
        }
    }
}
