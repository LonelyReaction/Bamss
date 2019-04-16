using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc; 

namespace BAMSS.Communication.IPC
{
    public class IPCClient<T> : IPCBase where T : MarshalByRefObject
    {
        public IPCClient(string channelName) : base(channelName) 
        {
            this._channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(this._channel, true);
        }
        public T Item(string name)
        {
            return Activator.GetObject(typeof(T), string.Format("ipc://{0}/{1}", this._channelName, name)) as T;
        }
    }
}
