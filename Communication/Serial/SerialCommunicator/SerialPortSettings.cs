using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace BAMSS.Communication.Serial
{
    public class SerialPortSettings
    {
		public string PortName {get; private set;}
        public int BaudRate { get; private set; }
        public Parity Parity { get; private set; }
        public int DataBits { get; private set; }
        public StopBits StopBits { get; private set; }
        public Handshake Handshake { get; private set; }
        public Encoding Encoding { get; private set; }
        public string NewLine { get; private set; }
        public SerialPortSettings(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake, Encoding encoding, string newLine)
        {
            this.PortName = portName;
            this.BaudRate = baudRate;
            this.Parity = parity;
            this.DataBits = dataBits;
            this.StopBits = stopBits;
            this.Handshake = handshake;
            this.Encoding = encoding;
            this.NewLine = newLine;
        }
    }
}
