using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMSS.Logging.Message
{
    public abstract class MessageLoggerBase : IMessageLogger
    {
        public abstract void Write(MessageLevel level, string senderInfo, string messageTitle, string messageBody, object option = null);
    }
}
