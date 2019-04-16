using System.Collections.Generic;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC通信定期監視プロセスクラス（抽象クラス）
    /// </summary>
    /// <remarks></remarks>
    public abstract class PlcWatchProcessor : PlcAccessor, IPlcWatchProcessor
    {
        /// <summary>
        /// 読込マップID
        /// </summary>
        protected string _readMapID;
        /// <summary>
        /// 読込マップ情報
        /// </summary>
        protected PlcDeviceMapInfo _readMapInfo;
        /// <summary>
        /// 読込マップ項目情報
        /// </summary>
        protected Dictionary<int, PlcDeviceMapItem> _readMapItems;
        /// <summary>
        /// 読込マップアクセスクラス
        /// </summary>
        protected PlcDeviceMapReadOnlyAccessor _readMapAccessor;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <param name="readMapID">読込マップID</param>
        /// <param name="plcConfLoader">設定情報ローダー</param>
        public PlcWatchProcessor(string connectionID, string readAddrMapID, string readItemMapID, PlcConfigLoader plcConfLoader)
            : base(connectionID, plcConfLoader)
        {
            this._readMapInfo = plcConfLoader.LoadPLCDeviceMapInfo(readAddrMapID);
            this._readMapItems = plcConfLoader.LoadPLCDeviceMapItems(readItemMapID);
            this._readMapAccessor = new PlcDeviceMapReadOnlyAccessor(this._plcConnectionInfo, this._readMapInfo, this._readMapItems);
        }
        /// <summary>
        /// 定周期処理
        /// </summary>
        /// <remarks></remarks>
        public abstract void WatchEvent();
        /// <summary>
        /// 処理名称取得
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public abstract string Name { get; }
    }
}
