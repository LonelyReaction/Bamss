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
    public class IPCServer<T> : IPCBase where T : MarshalByRefObject
    {
        private IDictionary<string, T> _dataList;
        public IPCServer(string channelName)
            : base(channelName)
        {
            this._channel = new IpcServerChannel(this._channelName);
            ChannelServices.RegisterChannel(this._channel, true);
            this._dataList = new Dictionary<string, T>();
        }
        public IPCServer(string channelName, IDictionary<string, T> dataList)
            : this(channelName)
        {
            foreach (var item in dataList)
            {
                this.Add(item.Key, item.Value);
            }
        }
        public void Add(string name, T data)
        {
            this._dataList.Add(name, data);
            RemotingServices.Marshal(data, name);
        }
        public T Items(string name)
        {
            return this._dataList[name];
        }
    }
}
