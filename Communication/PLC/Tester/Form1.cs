using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAMSS.MXComponent;

namespace Tester
{
    public partial class Form1 : Form
    {
        private PlcRWHSAccessor plcRWHS = new PlcRWHSAccessor("MasterPLC", "TEST_HS_MAP", "TEST_READ_ADDR_MAP", "TEST_READ_ITEM_MAP", "TEST_WRITE_ADDR_MAP", "TEST_WRITE_ITEM_MAP", PlcConfigrationLoaderFactory.LoadPlcConfigFromSQLServer());

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 3000;
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = 4;
            int[] data = { 0xffff, 0xffff, 0xffff, 0xffff };
            int decimalLength = 0;
            int[] workData = new int[size];
            Array.ConstrainedCopy(data, 0, workData, 0, size);
            ulong result = 0;
            for (int i = (workData.Length - 1); i >= 0; i += -1)
            {
                result <<= 16;
                result += (ulong)workData[i];
            }
            //result = 131073
            var step1 = result.ToString("X");       //"20001"       string
            var step2 = Convert.ToInt64(step1, 16);         //0x00020001    int
            var step3 = Math.Pow(10, decimalLength);        //1.0           int
            var step4 = Convert.ToString(step2 / step3);    //"131073"      string
            MessageBox.Show(string.Format("{0} : {1}", step4, Convert.ToString(result / Math.Pow(10, decimalLength))));
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var plcRWHS = new PlcRWHSAccessor("MasterPLC", "TEST_HS_MAP", "TEST_READ_MAP", "TEST_WRITE_MAP", PlcConfigrationLoaderFactory.LoadPlcConfigFromSQLServer());
            //plcRWHS.WatchEvent();
            //PlcConfigrationLoaderFactory.LoadPlcConfigFromSQLServer()
            //PlcConnectionInfo plcConn = new PlcConnectionInfo("PLCAccessTest", 0);
            //PlcDeviceMapReadWriteAccessor plcRW = new PlcDeviceMapReadWriteAccessor(plcConn,);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            plcRWHS.WatchEvent();
        }
    }
}
