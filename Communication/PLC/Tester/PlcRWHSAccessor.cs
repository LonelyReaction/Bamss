using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAMSS.MXComponent;

namespace Tester
{
    public class PlcRWHSAccessor : PlcHandShakeWaitReqReadWriteProcessor
    {
        private string _data1;
        private int _data2;
        private long _data3;
        private string _data4;
        public PlcRWHSAccessor(string connectionID, string handShakeID, string readAddrMapID, string readItemMapID, string writeAddrMapID, string writeItemMapID, PlcConfigLoader confLoader)
            : base(connectionID, handShakeID, readAddrMapID, readItemMapID, writeAddrMapID, writeItemMapID, confLoader, RequestResponseActionTiming.After, RequestResponseActionTiming.After) 
        { }
        protected override void RequestOn()
        {
            this._readMapAccessor.Read();
            this._data1 = this._readMapAccessor.GetItemValue(0);
            this._data2 = int.Parse(this._readMapAccessor.GetItemValue(1));
            this._data3 = long.Parse(this._readMapAccessor.GetItemValue(2));
            this._data4 = this._readMapAccessor.GetItemValue(3);
        }
        protected override void RequestOff()
        {
            this._writeMapAccessor.SetItemValue(0, _data1);
            this._writeMapAccessor.SetItemValue(1, _data2.ToString());
            this._writeMapAccessor.SetItemValue(2, _data3.ToString());
            this._writeMapAccessor.SetItemValue(3, _data4);
            this._writeMapAccessor.Write();
            this.ResponseToPLC(0);
        }
        public override string Name
        {
            get { return "PLCRead/Write HandShake Accessor"; }
        }
    }
}
