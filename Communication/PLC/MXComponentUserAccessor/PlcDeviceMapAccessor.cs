namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLCデバイスマップアクセス基底クラス
    /// [PlcAccessor←PlcDeviceAccessor]
    /// </summary>
    public abstract class PlcDeviceMapAccessor : PlcAccessor
    {
        /// <summary>
        /// デバイスタイプ（D,M等のデバイスの種類）
        /// </summary>
        protected string _plcDeviceType;
        /// <summary>
        /// 開始アドレス（デバイスタイプを含まない数値部分）
        /// </summary>
        protected string _plcStartAddress;
        /// <summary>
        /// マップのレジスタ値配列
        /// </summary>
        protected int[] _wordValues;
        /// <summary>
        /// マップのレジスタワード数
        /// </summary>
        protected int _deviceWords;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionInfo">接続情報クラスのインスタンス</param>
        /// <param name="deviceType">デバイスタイプ（D,M等のデバイスの種類）</param>
        /// <param name="startAddress">開始アドレス（デバイスタイプを含まない数値部分）</param>
        /// <param name="deviceWords">マップのレジスタワード数</param>
        public PlcDeviceMapAccessor(PlcConnectionInfo connectionInfo, string deviceType, string startAddress, int deviceWords)
            : base(connectionInfo)
        {
            this.CommonInitialize(deviceType, startAddress, deviceWords);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectLogicalNo">接続論理番号</param>
        /// <param name="connectionName">接続名</param>
        /// <param name="deviceType">デバイスタイプ（D,M等のデバイスの種類）</param>
        /// <param name="startAddress">開始アドレス（デバイスタイプを含まない数値部分）</param>
        /// <param name="deviceWords">マップのレジスタワード数</param>
        public PlcDeviceMapAccessor(int connectLogicalNo, string connectionName, string deviceType, string startAddress, int deviceWords)
            : base(new PlcConnectionInfo(connectionName, connectLogicalNo))
        {
            this.CommonInitialize(deviceType, startAddress, deviceWords);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionName">接続名</param>
        /// <param name="readAddrMapID">マップID</param>
        /// <param name="plcConfLoader">設定ローダー</param>
        public PlcDeviceMapAccessor(string connectionName, string readAddrMapID, PlcConfigLoader plcConfLoader)
            : base(plcConfLoader.LoadPLCConnectionInfo(connectionName))
        {
            var _readMapInfo = plcConfLoader.LoadPLCDeviceMapInfo(readAddrMapID);
            this.CommonInitialize(_readMapInfo.DeviceType, _readMapInfo.TopAddress, _readMapInfo.Size);
        }
        /// <summary>
        /// コンストラクタ共通処理
        /// </summary>
        /// <param name="deviceType">デバイスタイプ（D,M等のデバイスの種類）</param>
        /// <param name="startAddress">開始アドレス（デバイスタイプを含まない数値部分）</param>
        /// <param name="deviceWords">マップのレジスタワード数</param>
        private void CommonInitialize(string deviceType, string startAddress, int deviceWords)
        {
            this._plcDeviceType = deviceType;
            this._plcStartAddress = startAddress;
            this._deviceWords = deviceWords;
            this._wordValues = new int[this._deviceWords];
        }
        /// <summary>
        /// マップ一括読込処理(Waord読み)
        /// </summary>
        protected void Read()
        {
            this._plcAccessor.ReadWord(this.DeviceStartAddress, ref this._wordValues);
        }
        /// <summary>
        /// マップ一括読込処理(Bir読み)
        /// </summary>
        protected void ReadBit()
        {
            int _words = this._wordValues.Length / 16;
            int[] _wordList = new int[_words];
            this._plcAccessor.ReadWord(this.DeviceStartAddress, ref _wordList);
            int bitIndex = 0;
            for (int wordIndex = 0; wordIndex < _words; wordIndex++)
            {
                for(int shift = 0; shift < 16; shift++)
                {
                    this._wordValues[bitIndex++] = (_wordList[wordIndex] >> shift) & 1;
                }
            }
        }
        /// <summary>
        /// マップ一括書込処理
        /// </summary>
        protected void Write()
        {
            this._plcAccessor.WriteWord(this.DeviceStartAddress, this._wordValues);
        }
        /// <summary>
        /// マップ開始アドレス（デバイスタイプ＋番地形式）取得
        /// </summary>
        public string DeviceStartAddress
        {
            get { return string.Format("{0}{1}", this._plcDeviceType, this._plcStartAddress); }
        }
    }
}
