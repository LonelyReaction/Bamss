using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc; 

namespace BAMSS.Communication.IPC
{
    public abstract class IPCBase
    {
        protected string _channelName;
        protected IChannel _channel;
        public IPCBase(string channelName)
        {
            this._channelName = channelName;
        }
    }
}
