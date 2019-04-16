using System.Collections.Generic;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC通信受動H/S[ReadWrite]通信クラス（抽象クラス）
    /// </summary>
    /// <remarks></remarks>
    public abstract class PlcHandShakeWaitReqReadWriteProcessor : PlcHandShakeWaitReqReadProcessor
    {
        /// <summary>
        /// 書込マップID
        /// </summary>
        protected string _writeMapID;
        /// <summary>
        /// 書込マップ情報
        /// </summary>
        protected PlcDeviceMapInfo _writeMapInfo;
        /// <summary>
        /// 書込マップ項目情報
        /// </summary>
        protected Dictionary<int, PlcDeviceMapItem> _writeMapItems;
        /// <summary>
        /// 書込マップアクセスクラス
        /// </summary>
        protected PlcDeviceMapReadWriteAccessor _writeMapAccessor;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <param name="handShakeID">ハンドシェイクID</param>
        /// <param name="readAddrMapID">読込アドレスマップID</param>
        /// <param name="readItemMapID">読込項目マップID</param>
        /// <param name="writeAddrMapID">書込アドレスマップID</param>
        /// <param name="writeItemMapID">書込項目マップID</param>
        /// <param name="confLoader">設定情報ローダー</param>
        /// <param name="responseOnTiming">レスポンスOnのタイミング</param>
        /// <param name="responseOffTiming">レスポンスOffのタイミング</param>
        public PlcHandShakeWaitReqReadWriteProcessor(string connectionID, string handShakeID, string readAddrMapID, string readItemMapID, string writeAddrMapID, string writeItemMapID, PlcConfigLoader confLoader, RequestResponseActionTiming responseOnTiming, RequestResponseActionTiming responseOffTiming)
            : base(connectionID, handShakeID, readAddrMapID, readItemMapID, confLoader, responseOnTiming, responseOffTiming)
        {
            this._writeMapInfo = confLoader.LoadPLCDeviceMapInfo(writeAddrMapID);
            this._writeMapItems = confLoader.LoadPLCDeviceMapItems(writeItemMapID);
            this._writeMapAccessor = new PlcDeviceMapReadWriteAccessor(this._plcConnectionInfo, this._writeMapInfo, this._writeMapItems);
        }
    }
}
