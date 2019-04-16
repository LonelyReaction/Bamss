using System.Collections.Generic;
using System.Linq;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PlcDeviceMapReadWriteAccessor : PlcDeviceMapReadOnlyAccessor
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plcConnectionInfo">接続情報クラスインスタンス</param>
        /// <param name="deviceType">デバイスタイプ（D,M等のデバイスの種類）</param>
        /// <param name="startAddress">開始アドレス（デバイスタイプを含まない数値部分）</param>
        /// <param name="deviceWords">マップのレジスタワード数</param>
        /// <param name="mapItems">項目情報リスト</param>
        public PlcDeviceMapReadWriteAccessor(PlcConnectionInfo plcConnectionInfo, string deviceType, string startAddress, int deviceWords, Dictionary<int, PlcDeviceMapItem> mapItems)
            : base(plcConnectionInfo, deviceType, startAddress, deviceWords, mapItems)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plcConnectionInfo">接続情報クラスインスタンス</param>
        /// <param name="mapInfo">マップ情報</param>
        /// <param name="mapItems">マップ項目情報</param>
        public PlcDeviceMapReadWriteAccessor(PlcConnectionInfo plcConnectionInfo, PlcDeviceMapInfo mapInfo, Dictionary<int, PlcDeviceMapItem> mapItems)
            : base(plcConnectionInfo, mapInfo.DeviceType, mapInfo.TopAddress, mapInfo.Size, mapItems)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionName">接続名</param>
        /// <param name="readAddrMapID">マップ情報ID</param>
        /// <param name="readItemMapID">マップ項目情報ID</param>
        /// <param name="plcConfLoader">設定ローダー</param>
        public PlcDeviceMapReadWriteAccessor(string connectionName, string readAddrMapID, string readItemMapID, PlcConfigLoader plcConfLoader)
            : base(connectionName, readAddrMapID, readItemMapID, plcConfLoader)
        {
        }
        /// <summary>
        /// ワード値設定
        /// </summary>
        /// <param name="wordAddress">相対ワード位置(0ベース)</param>
        /// <param name="value">値</param>
        public void SetWordValue(int wordAddress, int value)
        {
            this._wordValues[wordAddress] = value;
        }
        /// <summary>
        /// 項目情報設定
        /// </summary>
        /// <param name="itemNo">項目番号</param>
        /// <param name="value">値</param>
        public void SetItemValue(int itemNo, string value)
        {
            this._mapItems[itemNo].Value = value;
        }
        /// <summary>
        /// 書込処理
        /// </summary>
        public new void Write()
        {
            List<int> resultWords = new List<int>();
            foreach (PlcDeviceMapItem item in this._mapItems.Values)
            {
                switch (item.Type)
                {
                    case PlcFieldType.Binary:
                        resultWords.AddRange(this._plcAccessor.ConvertBinaryAsWord((item.Value.Length < 1) ? "0" : int.Parse(item.Value).ToString("X"), item.Size, item.DecimalLength));
                        break;
                    case PlcFieldType.Ascii:
                        resultWords.AddRange(this._plcAccessor.ConvertAsciiAsWord(item.Value, item.Size));
                        break;
                    case PlcFieldType.BCD:
                        resultWords.AddRange(this._plcAccessor.ConvertBCDAsWord(item.Value, item.Size));
                        break;
                    default:
                        break;
                }
            }
            this._wordValues = resultWords.Take(this._deviceWords).ToArray();
            base.Write();
        }
    }
}
