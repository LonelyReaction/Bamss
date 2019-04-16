using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAMSS.UI
{
    public class ProgressMessageBase
    {
        protected ProgressBar _progBar;
        protected Control _messageControl;
        public ProgressMessageBase()
        {
            this._messageControl.Text = "";
        }
    }
}
