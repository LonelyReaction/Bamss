using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BAMSS.Data
{
    public abstract class ExecuteStoredProc
    {
        protected ExecuteStoredProc()
        {
        }
        public abstract void Execute(SqlClientAccessor db);
    }
}
