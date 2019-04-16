using System;
using System.IO.Ports;

namespace BAMSS.Communication.Serial
{
    public class CommSerialSingleReceiver : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public delegate void ReceiveDataEventHandler(string data);
        /// <summary>
        /// 
        /// </summary>
        public event ReceiveDataEventHandler ReceiveData;
        /// <summary>
        /// 
        /// </summary>
        private SerialPort _spComm = new SerialPort();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks></remarks>
        public CommSerialSingleReceiver(SerialPortSettings settings)
        {
            this._spComm.PortName = settings.PortName;
            this._spComm.BaudRate = settings.BaudRate;
            this._spComm.Parity = settings.Parity;
            this._spComm.DataBits = settings.DataBits;
            this._spComm.StopBits = settings.StopBits;
            this._spComm.Handshake = settings.Handshake;
            this._spComm.Encoding = settings.Encoding;
            this._spComm.NewLine = settings.NewLine;

            this._spComm.DataReceived += this._spcCommunicator_DataReceived;
        }
        /// <summary>
        /// シリアルポートオープン
        /// </summary>
        /// <remarks></remarks>
        public void Open() { if (!this._spComm.IsOpen) this._spComm.Open(); }
        /// <summary>
        /// ポートクローズ
        /// </summary>
        /// <remarks></remarks>
        public void Close() { if (this._spComm.IsOpen) this._spComm.Close(); }
        /// <summary>
        /// シリアルポートデータ受信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void _spcCommunicator_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (ReceiveData != null) ReceiveData(this._spComm.ReadExisting().Replace(this._spComm.NewLine, "\r\n"));
            }
            catch { }
        }
        #region "IDisposable Support"
            // 重複する呼び出しを検出するには
            private bool disposedValue;

            // IDisposable
            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing)
                    {
                        //マネージ状態を破棄します (マネージ オブジェクト)。
                        this.Close();
                        this._spComm.Dispose();
                    }
                }
                this.disposedValue = true;
            }

            // TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
            //Protected Overrides Sub Finalize()
            //    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
            //    Dispose(False)
            //    MyBase.Finalize()
            //End Sub

            // このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
            public void Dispose()
            {
                // このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        #endregion
    }
}
