using BAMSS.Data;
using BAMSS.Logging.Message;
using System.Data;
using System.Data.SqlClient;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLCアクセス設定情報クラス(SQLServerからDataTableを介して設定情報を取得するクラス)
    /// [PlcConfigLoader←PlcConfigLoaderDataTable←PlcConfigLoaderSQLServer]
    /// </summary>
    public class PlcConfigLoaderSQLServer : PlcConfigLoaderDataTable
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private IMessageLogger _logger;
        /// <summary>
        /// 接続文字列
        /// </summary>
        private string _connectionString;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbConnectionString"></param>
        public PlcConfigLoaderSQLServer(string dbConnectionString, IMessageLogger logger = null)
        {
            this._connectionString = dbConnectionString;
            this._logger = logger;
        }
        /// <summary>
        /// 接続設定情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="connectionID">接続ID</param>
        /// <returns></returns>
        protected sealed override DataTable GetPLCConnectionInfo(string plcConnectionID)
        {
            using (SqlClientAccessor db = new SqlClientAccessor(this._connectionString))
            {
                db.Open();
                SqlCommand command = (SqlCommand)db.GetCommand(false);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PGetPlcConnectionInfo";
                command.Parameters.Clear();
                command.Parameters.Add("@connectionID", SqlDbType.VarChar).Value = plcConnectionID;
                return db.GetDataTable(command, false);
            }
        }
        /// <summary>
        /// ハンドシェイク情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="handShakeID">ハンドシェイクID</param>
        /// <returns></returns>
        protected sealed override DataTable GetPLCHandShakeInfo(string handShakeID)
        {
            using (SqlClientAccessor db = new SqlClientAccessor(this._connectionString))
            {
                db.Open();
                SqlCommand command = (SqlCommand)db.GetCommand(false);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PGetPlcHandShakeInfo";
                command.Parameters.Clear();
                command.Parameters.Add("@handShakeID", SqlDbType.VarChar).Value = handShakeID;
                return db.GetDataTable(command, false);
            }
        }
        /// <summary>
        /// データマップ情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        protected sealed override DataTable GetPLCDeviceMapInfo(string mapID)
        {
            using (SqlClientAccessor db = new SqlClientAccessor(this._connectionString))
            {
                db.Open();
                SqlCommand command = (SqlCommand)db.GetCommand(false);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PGetPlcDeviceMapInfo";
                command.Parameters.Clear();
                command.Parameters.Add("@deviceMapID", SqlDbType.VarChar).Value = mapID;
                return db.GetDataTable(command, false);
            }
        }
        /// <summary>
        /// データ項目情報をDataTableとして取得するメソッド
        /// </summary>
        /// <param name="mapID">データマップID</param>
        /// <returns></returns>
        protected sealed override DataTable GetPLCDeviceMapItems(string mapID)
        {
            using (SqlClientAccessor db = new SqlClientAccessor(this._connectionString))
            {
                db.Open();
                SqlCommand command = (SqlCommand)db.GetCommand(false);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PGetPlcDeviceMapItems";
                command.Parameters.Clear();
                command.Parameters.Add("@deviceMapID", SqlDbType.VarChar).Value = mapID;
                return db.GetDataTable(command, false);
            }
        }
    }
}
