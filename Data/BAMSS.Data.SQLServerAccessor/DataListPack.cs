using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BAMSS.Data
{
    public abstract class DataListPack
    {
        protected string _connectionString;
        protected List<IList<RecordBase>> _tables;
        protected IList<RecordBase> Tables(int no) { return _tables[no]; }

        protected DataListPack(string connectionString) { this._connectionString = connectionString; }

        protected void Initialize(List<IList<RecordBase>> tables) { this._tables = tables; }
        protected abstract DataSet GetDataSet();
        protected abstract void BuildFromDataSet(DataSet ds);

        public void Build() { this.BuildFromDataSet(this.GetDataSet()); }
    }
}
