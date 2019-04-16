using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc; 
using System.Windows.Forms;

namespace BAMSS.Communication.IPC
{
    public partial class TesterServer : Form
    {
        private IPCServer<TestData> _ipc;
        public TesterServer()
        {
            InitializeComponent();
            this._ipc = new IPCServer<TestData>("Test");
            this._ipc.Add("001", new TestData());
            this._ipc.Add("00A", new TestData());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this._ipc.Items("001").Count++;
            this.button1.Text = this._ipc.Items("001").Count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this._ipc.Items("00A").Count++;
            this.button2.Text = this._ipc.Items("00A").Count.ToString();
        }
    }
}
