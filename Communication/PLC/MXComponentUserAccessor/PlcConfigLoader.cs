using System;
using System.Collections.Generic;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLCアクセス設定情報基底クラス
    /// </summary>
    public abstract class PlcConfigLoader
    {
        /// <summary>
        /// フィールドタイプ列挙値取得関数（設定文字列値）
        /// </summary>
        /// <param name="attrName">フィールドタイプ(設定文字列値)</param>
        /// <returns></returns>
        protected static PlcFieldType GetPLCDeviceItemAttribute(string attrName)
        {
            switch (attrName.ToUpper())
            {
                case "BINARY":
                    return PlcFieldType.Binary;
                case "ASCII":
                    return PlcFieldType.Ascii;
                case "BCD":
                    return PlcFieldType.BCD;
            }
            throw new ArgumentException(string.Format("PLCメモリデータ属性の取得に失敗しました。属性名：[{0}]", attrName));
        }
        /// <summary>
        /// 接続設定情報のローダーメソッド
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <returns></returns>
        public abstract PlcConnectionInfo LoadPLCConnectionInfo(string connectionID);
        /// <summary>
        /// ハンドシェイク情報のローダーメソッド
        /// </summary>
        /// <param name="handShakeID">ハンドシェイクID</param>
        /// <returns></returns>
        public abstract PlcHandShakeInfo LoadPLCHandShakeInfo(string handShakeID);
        /// <summary>
        /// データマップ情報のローダーメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        public abstract PlcDeviceMapInfo LoadPLCDeviceMapInfo(string mapID);
        /// <summary>
        /// データ項目情報のローダーメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        public abstract Dictionary<int, PlcDeviceMapItem> LoadPLCDeviceMapItems(string mapID);
    }
}
