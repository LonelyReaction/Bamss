using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC要求管理クラス
    /// </summary>
    /// <remarks></remarks>
    public class MXComponentCoreBitValueReader
    {
        private Dictionary<string, bool> _bitValue;
        private List<string> _addresses;
        private MXComponentCoreAccessor _plc;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="bitAddresses"></param>
        public MXComponentCoreBitValueReader(MXComponentCoreAccessor plc, List<string> bitAddresses)
        {
            this._plc = plc;
            this._addresses = bitAddresses;
            this._bitValue = new Dictionary<string, bool>();
            foreach (string address in this._addresses)
            {
                this._bitValue.Add(address, false);
            }
        }
        /// <summary>
        /// 再読込処理
        /// </summary>
        public void Refresh()
        {
            List<bool> bitValue = this._plc.ReadBit(this._addresses);
            for (int addressNo = 0; addressNo <= (this._addresses.Count - 1); addressNo += 1)
            {
                this._bitValue[this._addresses[addressNo]] = bitValue[addressNo];
            }
        }
        /// <summary>
        /// ビット値取得
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool BitValue(string address)
        {
            return _bitValue[address]; 
        }
    }
}
