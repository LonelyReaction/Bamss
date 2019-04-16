using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BAMSS.Serializer;

namespace BAMSS.Data
{
    public abstract class UpdateSample1 : ExecuteStoredProc
    {
        private string _shijiNo;
        private int? _groupNo;
        private IList<RecSample_001> _list;
        public UpdateSample1(string connectionString, string shijiNo, int? groupNo, IList<RecSample_001> list)
        {
            this._shijiNo = shijiNo;
            this._groupNo = groupNo;
            this._list = list;
        }
        public override void Execute(SqlClientAccessor db)
        {
            string message = "";
            int updateRecs;
            SqlCommand cmd = (SqlCommand)db.GetCommand(true);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PUpdate_Sapmle1";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@shijiNo", SqlDbType.VarChar).Value = this._shijiNo;
            if (this._groupNo == null) cmd.Parameters.Add("@groupNo", SqlDbType.Int).Value = DBNull.Value;
            else cmd.Parameters.Add("@groupNo", SqlDbType.Int).Value = this._groupNo;
            cmd.Parameters.SetListParam("@list", "SRECSAMPLE", (IList<RecordBase>)this._list);
            cmd.Parameters.Add("ReturnValue", SqlDbType.Int);
            cmd.Parameters["ReturnValue"].Direction = ParameterDirection.ReturnValue;
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read()) message = dr["MESSAGE"].ToString();
            }
            updateRecs = (int)cmd.Parameters["ReturnValue"].Value;
            if (updateRecs < 1) throw new Exception(string.Format("ユーザーメモリの更新に失敗しました。\r\n{0}", message));
        }
    }
}
