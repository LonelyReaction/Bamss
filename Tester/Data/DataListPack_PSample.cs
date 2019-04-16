using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BAMSS.Data
{
    public class DataListPack_PSample : DataListPack
    {
        private string _shijiNo;
        private int? _groupNo;
        private IList<RecSample_001> _list;
        public DataListPack_PSample(string connectionString, string shijiNo, int? groupNo, IList<RecSample_001> list)
            : base(connectionString)
        {
            this._shijiNo = shijiNo;
            this._groupNo = groupNo;
            this._list = list;
            this._tables = new List<IList<RecordBase>>() { null, null };
        }
        protected override DataSet GetDataSet()
        {
            using (SqlClientAccessor db = new SqlClientAccessor(this._connectionString))
            {
                db.Open();
                SqlCommand cmd = (SqlCommand)db.GetCommand(false);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PGetListSample";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@shijiNo", SqlDbType.VarChar).Value = this._shijiNo;
                if (this._groupNo == null) cmd.Parameters.Add("@groupNo", SqlDbType.Int).Value = DBNull.Value;
                else cmd.Parameters.Add("@groupNo", SqlDbType.Int).Value = this._groupNo;
                cmd.Parameters.SetListParam("@list", "SRECSAMPLE", (IList<RecordBase>)this._list);
                return db.GetDataSet(cmd, false);
            }
        }
        protected override void BuildFromDataSet(DataSet ds)
        {
            this._tables[0] = (IList<RecordBase>)ds.Tables[0].BuildList<RecSample_001>();
            this._tables[1] = (IList<RecordBase>)ds.Tables[1].BuildList<RecSample_002>();
        }
        public List<RecSample_001> Sample1 { get { return (List<RecSample_001>)this._tables[0]; } }
        public List<RecSample_002> Sample2 { get { return (List<RecSample_002>)this._tables[1]; } }
    }
}
