using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMSS.Diagnostic
{
    public interface IApplicationDiagRecord
    {
        string Header();
        string Record();
    }
}
