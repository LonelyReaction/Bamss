using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMSS.UI.SwapPage
{
    public class SwapPageControlException : Exception
    {
        public SwapPageBase Page { get; private set; }
        public SwapPageControlException(SwapPageBase page, string message)
            :base(message)
        {
            this.Page = page;
        }
    }
}
