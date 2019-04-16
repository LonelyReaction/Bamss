using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BAMSS.Logging.Message;

namespace BAMSS.Data
{
    public class TransactionStoredProc
    {
        private List<ExecuteStoredProc> _procs;
        private IMessageLogger _logger;
        private string _connectionString;
        public TransactionStoredProc(string connectionString, List<ExecuteStoredProc> procs, IMessageLogger logger = null) 
        {
            this._connectionString = connectionString;
            this._procs = procs;
            this._logger = logger;
        }
        public void Add(ExecuteStoredProc proc) { this._procs.Add(proc); }
        public void Execute()
        {
            using (SqlClientAccessor db = new SqlClientAccessor(this._connectionString, this._logger))
            { 
                db.Open();
                db.BeginTrans();
                try
                {
                    this._procs.ForEach(c => c.Execute(db));
                    db.CommitTrans();
                }
                catch
                {
                    db.RollbackTrans();
                    throw;
                }
            }
        }
    }
}
