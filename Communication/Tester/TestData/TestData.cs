using System;

namespace BAMSS.Communication.IPC
{
    /// <summary>
    /// プロセス間通信サンプルのためのオブジェクト.
    /// </summary>
    public class TestData : MarshalByRefObject
    {
        public int Count { get; set; }
    }
}
