namespace BAMSS.MXComponent
{
    /// <summary>
    /// デバイスマップ情報クラス
    /// </summary>
    public class PlcDeviceMapInfo
    {
        private int _blockSeq;
        private string _deviceType;
        private string _topAddress;
        private int _size;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="blockSeq">ブロックシーケンス【現在未使用】</param>
        /// <param name="deviceType">デバイスタイプ（D,M等のデバイスの種類）</param>
        /// <param name="address">開始アドレス（デバイスタイプを含まない数値部分）</param>
        /// <param name="size">マップサイズ（ワード数）</param>
        public PlcDeviceMapInfo(int blockSeq, string deviceType, string address, int size)
        {
            this._blockSeq = blockSeq;
            this._deviceType = deviceType;
            this._topAddress = address;
            this._size = size;
        }
        /// <summary>
        /// ブロックシーケンス【現在未使用】
        /// </summary>
        public int BlockSeq
        {
            get { return this._blockSeq; }
        }
        /// <summary>
        /// デバイスタイプ（D,M等のデバイスの種類）
        /// </summary>
        public string DeviceType
        {
            get { return this._deviceType; }
        }
        /// <summary>
        /// 開始アドレス（デバイスタイプを含まない数値部分）
        /// </summary>
        public string TopAddress
        {
            get { return this._topAddress; }
        }
        /// <summary>
        /// マップサイズ（ワード数）
        /// </summary>
        public int Size
        {
            get { return this._size; }
        }
    }
}
