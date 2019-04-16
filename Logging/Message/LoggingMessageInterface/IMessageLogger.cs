using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMSS.Logging.Message
{
    public interface IMessageLogger
    {
        void Write(MessageLevel level, string information, string messageTitle, string messageBody, object option = null);
    }
}
