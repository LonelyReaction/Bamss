using System.Collections.Generic;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLCデバイスアクセス読取専用アクセスクラス
    /// [PlcAccessor←PlcDeviceMapAccessor←PlcDeviceMapReadOnlyAccessor]
    /// </summary>
    public class PlcDeviceMapReadOnlyAccessor : PlcDeviceMapAccessor
    {
        /// <summary>
        /// マップ内のPlcDeviceMapItemリスト（キーは項目番号）
        /// </summary>
        protected Dictionary<int, PlcDeviceMapItem> _mapItems;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plcConnectionInfo">接続情報クラスインスタンス</param>
        /// <param name="deviceType">デバイスタイプ（D,M等のデバイスの種類）</param>
        /// <param name="startAddress">開始アドレス（デバイスタイプを含まない数値部分）</param>
        /// <param name="deviceWords">マップのレジスタワード数</param>
        /// <param name="mapItems">項目情報リスト</param>
        public PlcDeviceMapReadOnlyAccessor(PlcConnectionInfo plcConnectionInfo, string deviceType, string startAddress, int deviceWords, Dictionary<int, PlcDeviceMapItem> mapItems)
            : base(plcConnectionInfo, deviceType, startAddress, deviceWords)
        {
            this._mapItems = mapItems;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plcConnectionInfo">接続情報クラスインスタンス</param>
        /// <param name="mapInfo">マップ情報</param>
        /// <param name="mapItems">マップ項目情報</param>
        public PlcDeviceMapReadOnlyAccessor(PlcConnectionInfo plcConnectionInfo, PlcDeviceMapInfo mapInfo, Dictionary<int, PlcDeviceMapItem> mapItems)
            : base(plcConnectionInfo, mapInfo.DeviceType, mapInfo.TopAddress, mapInfo.Size)
        {
            this._mapItems = mapItems;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionName">接続名</param>
        /// <param name="readAddrMapID">マップ情報ID</param>
        /// <param name="readItemMapID">マップ項目情報ID</param>
        /// <param name="plcConfLoader">設定ローダー</param>
        public PlcDeviceMapReadOnlyAccessor(string connectionName, string readAddrMapID, string readItemMapID, PlcConfigLoader plcConfLoader)
            : base(connectionName, readAddrMapID, plcConfLoader)
        {
            this._mapItems = plcConfLoader.LoadPLCDeviceMapItems(readItemMapID);
        }
        /// <summary>
        /// ワード値取得
        /// </summary>
        /// <param name="wordAddress">相対ワード位置(0ベース)</param>
        /// <returns></returns>
        public int GetWordValue(int wordAddress)
        {
            return this._wordValues[wordAddress];
        }
        /// <summary>
        /// 項目情報取得
        /// </summary>
        /// <param name="itemNo">項目番号</param>
        /// <returns></returns>
        public string GetItemValue(int itemNo)
        {
            return this._mapItems[itemNo].Value;
        }
        /// <summary>
        /// 読込処理(Waord読み)
        /// </summary>
        public new void Read()
        {
            base.Read();
            for (int i = 0; i <= (this._mapItems.Count - 1); i += 1)
            {
                switch (this._mapItems[i].Type)
                {
                    case PlcFieldType.Binary:
                        this._mapItems[i].Value = this._plcAccessor.ConvWordAsBinary(int.Parse(this._mapItems[i].Address), this._mapItems[i].Size, this._wordValues, !this._mapItems[i].UnSigned, this._mapItems[i].DecimalLength);
                        break;
                    case PlcFieldType.Ascii:
                        this._mapItems[i].Value = this._plcAccessor.ConvWordAsAscii(int.Parse(this._mapItems[i].Address), this._mapItems[i].Size, this._wordValues);
                        break;
                    case PlcFieldType.BCD:
                        this._mapItems[i].Value = this._plcAccessor.ConvWordAsBcd(int.Parse(this._mapItems[i].Address), this._mapItems[i].Size, this._wordValues, this._mapItems[i].DecimalLength);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 読込処理(Bir読み)
        /// </summary>
        public new void ReadBit()
        {
            base.ReadBit();
            for (int i = 0; i <= (this._mapItems.Count - 1); i += 1)
            {
                this._mapItems[i].Value = this._plcAccessor.ConvWordAsBinary(int.Parse(this._mapItems[i].Address), this._mapItems[i].Size, this._wordValues, !this._mapItems[i].UnSigned, this._mapItems[i].DecimalLength);
            }
        }
    }
}
