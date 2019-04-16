using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows.Forms;

namespace BAMSS.Communication.IPC
{
    public partial class TesterClient : Form
    {
        private IPCClient<TestData> _ipc;
        public TesterClient()
        {
            InitializeComponent();
            this._ipc = new IPCClient<TestData>("Test");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //カウントを1増やす
            this._ipc.Item("001").Count++;
            button1.Text = this._ipc.Item("001").Count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this._ipc.Item("00A").Count++;
            button2.Text = this._ipc.Item("00A").Count.ToString();
        }
    }
}
