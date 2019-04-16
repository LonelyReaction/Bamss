using System.Collections.Generic;
using System.Data;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLCアクセス設定情報基底クラス(DataTableを介して設定情報を取得するクラスの基底クラス)
    /// [PlcConfigLoader←PlcConfigLoaderDataTable]
    /// </summary>
    public abstract class PlcConfigLoaderDataTable : PlcConfigLoader
    {
        /// <summary>
        /// 接続設定情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <returns></returns>
        protected abstract DataTable GetPLCConnectionInfo(string connectionID);
        /// <summary>
        /// ハンドシェイク情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="handShakeID">ハンドシェイクID</param>
        /// <returns></returns>
        protected abstract DataTable GetPLCHandShakeInfo(string handShakeID);
        /// <summary>
        /// データマップ情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        protected abstract DataTable GetPLCDeviceMapInfo(string mapID);
        /// <summary>
        /// データ項目情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        protected abstract DataTable GetPLCDeviceMapItems(string mapID);
        /// <summary>
        /// 接続設定情報のローダーメソッド
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <returns></returns>
        public override PlcConnectionInfo LoadPLCConnectionInfo(string connectionID)
        {
            DataTable dt = this.GetPLCConnectionInfo(connectionID);
            return new PlcConnectionInfo(dt.Rows[0]["CONNECTION_NAME"].ToString(), int.Parse(dt.Rows[0]["LOGICAL_CONNECT_NO"].ToString()));
        }
        /// <summary>
        /// ハンドシェイク情報のローダーメソッド
        /// </summary>
        /// <param name="handShakeID">ハンドシェイクID</param>
        /// <returns></returns>
        public override PlcHandShakeInfo LoadPLCHandShakeInfo(string handShakeID)
        {
            DataTable dt = this.GetPLCHandShakeInfo(handShakeID);
            return new PlcHandShakeInfo(dt.Rows[0]["REQUEST_ADDRESS"].ToString(), dt.Rows[0]["RESPONSE_ADDRESS"].ToString(), int.Parse(dt.Rows[0]["SCAN_INTERVAL"].ToString()));
        }
        /// <summary>
        /// データマップ情報のローダーメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        public override PlcDeviceMapInfo LoadPLCDeviceMapInfo(string mapID)
        {
            DataTable dt = this.GetPLCDeviceMapInfo(mapID);
            return new PlcDeviceMapInfo(int.Parse(dt.Rows[0]["BLOCK_SEQ"].ToString()), dt.Rows[0]["DEVICE_TYPE"].ToString(), dt.Rows[0]["TOP_ADDRESS"].ToString(), int.Parse(dt.Rows[0]["WORD_SIZE"].ToString()));
        }
        /// <summary>
        /// データ項目情報のローダーメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        public override Dictionary<int, PlcDeviceMapItem> LoadPLCDeviceMapItems(string mapID)
        {
            DataTable dt = this.GetPLCDeviceMapItems(mapID);
            Dictionary<int, PlcDeviceMapItem> mapItems = new Dictionary<int, PlcDeviceMapItem>();
            for (int i = 0; i <= (dt.Rows.Count - 1); i += 1)
            {
                mapItems.Add(i, new PlcDeviceMapItem(dt.Rows[i]["ITEM_NAME"].ToString(), dt.Rows[i]["ADDRESS_OFFSET"].ToString(), int.Parse(dt.Rows[i]["SIZE"].ToString()), GetPLCDeviceItemAttribute(dt.Rows[i]["ATTRIBUTE"].ToString()), int.Parse(dt.Rows[i]["DECIMAL_SIZE"].ToString()), (int.Parse(dt.Rows[i]["UNSIGNED"].ToString()) == 1)));
            }
            return mapItems;
        }
    }
}
