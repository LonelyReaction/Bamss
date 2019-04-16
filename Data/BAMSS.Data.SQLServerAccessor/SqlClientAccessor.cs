using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using BAMSS.Logging.Message;

namespace BAMSS.Data
{
    /// <summary>
    /// DB操作クラス
    /// </summary>
    /// <remarks></remarks>
    public class SqlClientAccessor : IDisposable
    {
        private static int RETRY_MAX = 5;
        private static int RETRY_INTERVAL = 200;
        private static readonly string _programFilePath = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string _programFileName = Path.GetFileNameWithoutExtension(_programFilePath.Split('.')[0]);
        /// <summary>
        /// メッセージロガーオブジェクト
        /// </summary>
        private IMessageLogger _logger;
        /// <summary>
        /// 処理結果ステータス
        /// </summary>
        /// <remarks></remarks>
        public enum DbResultState
        {
            Done = 0,           //通常（エラーなし）
            CannotOpen,         //DBオープンエラー
            AlreadyOpen,        //すでにオープンしている
            CannotClose,        //DBクローズエラー
            TransNotBegin,      //トランザクションが開始されていない
            TransAlreadyBegin   //トランザクションすでに開始されている
        }
        /// <summary>
        /// 接続ステータス
        /// </summary>
        /// <remarks></remarks>
        public enum DbConnectState
        {
            NoObject,           //接続オブジェクトが存在しない
            Closed,             //閉じている
            Opened,             //すでにオープンしている
            OpenedWithTrans     //すでにオープンしておりトランザクション中
        }
        //コマンドタイムアウト(秒)
        private int _commandTimeout = 60;
        //接続文字列
        public string _connectionString;
        //DB接続オブジェクト
        private SqlConnection _connection;
        //Transactionオブジェクト
        private SqlTransaction _transaction;
        /// <summary>
        /// 現在の接続を取得する
        /// </summary>
        /// <value></value>
        /// <returns>Connectionオブジェクト</returns>
        /// <remarks></remarks>
        public string ConnectionString{ get { return _connectionString; } }
        /// <summary>
        /// 現在の接続を取得する
        /// </summary>
        /// <value></value>
        /// <returns>Connectionオブジェクト</returns>
        /// <remarks></remarks>
        public SqlConnection Connection { get { return _connection; } }
        /// <summary>
        /// 接続状態を取得する
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DbConnectState ConnectState
        {
            get
            {
                if (this._connection == null) return DbConnectState.NoObject;
                else if (_connection.State == ConnectionState.Closed) return DbConnectState.Closed;
                else if (_transaction == null) return DbConnectState.Opened;
                else return DbConnectState.OpenedWithTrans;
            }
        }
        /// <summary>
        ///　コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="outputExecuteLog">処理ログ出力指定</param>
        public SqlClientAccessor(string connectionString, IMessageLogger logger = null)
        {
            this._connectionString = connectionString;
            this._logger = logger;
        }
        /// <summary>
        ///　コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="commandTimeout">コマンドタイムアウト秒数</param>
        /// <param name="outputExecuteLog">処理ログ出力指定</param>
        public SqlClientAccessor(string connectionString, int commandTimeout, IMessageLogger logger = null)
            : this(connectionString, logger)
        {
            this._commandTimeout = commandTimeout;
        }
        /// <summary>
        /// 処理ログ出力処理
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        private void WriteProcessLog(string message, params object[] args)
        {
            if (this._logger == null) return;
            //throw new NotImplementedException("SqlClientAccessor::WriteProcessLog");
            var _message = string.Format("{0}/接続設定:[{1}]", string.Format(message, args), this._connectionString);
            this._logger.Write(MessageLevel.Debug, null, null, _message);
        }
        private void WriteProcessLogWithMessageBox(string message, params object[] args)
        {
            if (this._logger == null) return;
            //throw new NotImplementedException("SqlClientAccessor::WriteProcessLog");
            var _message = string.Format("{0}/接続設定:[{1}]", string.Format(message, args), this._connectionString);
            this._logger.Write(MessageLevel.Debug, null, null, _message);
            MessageBox.Show(message, "DBアクセスエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        /// <summary>
        /// データベースオープン
        /// </summary>
        /// <param name="forceOpen">接続状態にかかわらず接続を一旦破棄して再接続を行う場合は[true]、切断状態時にのみ接続する場合は[false]を指定する。</param>
        /// <returns>処理結果ステータス</returns>
        public DbResultState Open(bool forceOpen = false)
        {
            string message;
            for (var reTryCount = 0; reTryCount <= RETRY_MAX; reTryCount++)
            {
                try
                {
                    //接続が閉じている場合のみ、オープンが可能
                    if ((forceOpen) || (this.ConnectState == DbConnectState.Closed) || (this.ConnectState == DbConnectState.NoObject))
                    {
                        if (this._connection != null) this._connection.Dispose();
                        this._connection = new SqlConnection(this._connectionString);
                        this._connection.Open();
                        //this.WriteProcessLog("データベースのオープン処理正常終了。");
                        return DbResultState.Done;
                    }
                    else if (this.ConnectState != DbConnectState.Opened)
                    {
                        //string message = string.Format("データベースがクローズ状態でありません。状態：[{0}]", this.ConnectState.ToString());
                        //this.WriteProcessLog(message);
                        //throw new InvalidOperationException(message);
                        message = string.Format("データベースがクローズ状態でもオープン状態でもないので再接続します。状態：[{0}]", this.ConnectState.ToString());
                        this.WriteProcessLog(message);
                        if (this._connection != null) this._connection.Dispose();
                        this._connection = new SqlConnection(this._connectionString);
                        this._connection.Open();
                        return DbResultState.Done;
                    }
                    else
                    {
                        this.WriteProcessLog("データベースは既にオープンされていました。");
                        return DbResultState.Done;
                    }
                }
                catch (SqlException ex)
                {
                    if (reTryCount >= RETRY_MAX)
                    {
                        message = string.Format("データベースのオープンに失敗しました、リトライオーバーしました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                        this.WriteProcessLogWithMessageBox(message);
                        throw new Exception(message, ex);
                    }
                    else if (reTryCount < 1)
                    {   //リトライ時は1回だけロギング
                        message = string.Format("データベースのオープンに失敗しました、リトライします。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                        this.WriteProcessLog(message);
                    }
                }
                catch (Exception ex)
                {
                    if (reTryCount >= RETRY_MAX)
                    {
                        message = string.Format("データベースのオープンに失敗しました、リトライオーバーしました。(Exception) Message:[{0}]", ex.Message);
                        this.WriteProcessLogWithMessageBox(message);
                        throw new Exception(message, ex);
                    }
                    else if (reTryCount < 1)
                    {   //リトライ時は1回だけロギング
                        message = string.Format("データベースのオープンに失敗しました、リトライします。(Exception) Message:[{0}]", ex.Message);
                        this.WriteProcessLog(message);
                    }
                }
                System.Threading.Thread.Sleep(RETRY_INTERVAL);
            }
            message = "データベースのオープンに失敗しました。";
            this.WriteProcessLogWithMessageBox(message);
            throw new Exception(message);
        }
        /// <summary>
        /// DBクローズ
        /// </summary>
        /// <returns>処理結果ステータス</returns>
        /// <remarks></remarks>
        public DbResultState Close()
        {
            if ((this.ConnectState == DbConnectState.Opened))
            {
                try
                {
                    this._connection.Close();
                    this._connection.Dispose();
                    this._connection = null;
                    return DbResultState.Done;
                }
                catch (SqlException ex)
                {
                    string message = string.Format("データベースのクローズに失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                    this.WriteProcessLog(message);
                    throw new Exception(message, ex);
                }
            }
            else if ((this.ConnectState == DbConnectState.OpenedWithTrans))
            {
                throw new Exception("トランザクションがCommitまたはRollbackされていません。");
            }
            else
            {
                throw new Exception("DBが接続されていないか、開いていません。");
            }
        }
        /// <summary>
        /// DBの強制クローズ
        /// </summary>
        /// <param name="withCommit">処理中のトランザクションをコミットするかどうか</param>
        /// <returns>処理結果</returns>
        /// <remarks></remarks>
        public DbResultState ForceClose(bool withCommit)
        {
            try
            {
                if (this.ConnectState == DbConnectState.OpenedWithTrans)
                {
                    if (withCommit) this._transaction.Commit();
                    else this._transaction.Rollback();
                }
                if (this.ConnectState == DbConnectState.Opened) this._connection.Close();
                this._connection = null;
                return DbResultState.Done;
            }
            catch (SqlException ex)
            {
                string message = string.Format("データベースの強制クローズに失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                this.WriteProcessLog(message);
                throw new Exception(message, ex);
            }
        }
        ///// <summary>
        ///// データテーブルの取得
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns>DataTableオブジェクト</returns>
        ///// <remarks></remarks>
        //public DataTable GetDataTable(string sql, bool useTransaction = false)
        //{
        //    DataTable aDataTable = new DataTable();
        //    try
        //    {
        //        if ((this.ConnectState == DbConnectState.OpenedWithTrans) || (this.ConnectState == DbConnectState.Opened))
        //        {
        //            SqlCommand command = (SqlCommand)this.GetCommand(useTransaction);
        //            command.CommandText = sql;
        //            command.CommandTimeout = this._commandTimeout;
        //            using (SqlDataAdapter sqlDa = new SqlDataAdapter(command))
        //            {
        //                this.WriteProcessLog(this.GetStartLogText(command));
        //                sqlDa.Fill(aDataTable);
        //                this.WriteProcessLog(this.GetEndLogText(command, aDataTable));
        //            }
        //            return aDataTable;
        //        }
        //        else
        //        {
        //            throw new Exception("DBが接続されていないか、開いていません。");
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        string message = string.Format("データベース：データテーブルの取得に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
        //        this.WriteProcessLog(message);
        //        throw new Exception(message, ex);
        //    }
        //}
        /// <summary>
        /// データテーブルの取得
        /// </summary>
        /// <param name="command"></param>
        /// <returns>DataTableオブジェクト</returns>
        /// <remarks></remarks>
        public DataTable GetDataTable(SqlCommand command, bool useTransaction = false)
        {
            string message;
            DataTable aDataTable = new DataTable();
            for (var reTryCount = 0; reTryCount <= RETRY_MAX; reTryCount++)
            {
                try
                {
                    if ((this.ConnectState == DbConnectState.OpenedWithTrans) || (this.ConnectState == DbConnectState.Opened))
                    {
                        using (SqlDataAdapter sqlDa = new SqlDataAdapter(command))
                        {
                            this.WriteProcessLog(this.GetStartLogText(command));
                            sqlDa.Fill(aDataTable);
                            this.WriteProcessLog(this.GetEndLogText(command, aDataTable));
                        }
                        return aDataTable;
                    }
                    else
                    {
                        //throw new Exception("DBが接続されていないか、開いていません。");
                    }
                }
                catch (SqlException ex)
                {
                    if (reTryCount >= RETRY_MAX)
                    {
                        message = string.Format("データベース：データテーブルの取得に失敗しました、リトライオーバーしました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                        this.WriteProcessLogWithMessageBox(message);
                        throw new Exception(message, ex);
                    }
                    else if (reTryCount < 1)
                    {   //リトライ時は1回だけロギング
                        message = string.Format("データベース：データテーブルの取得に失敗しました、リトライします。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                        this.WriteProcessLog(message);
                    }
                }
                catch (Exception ex)
                {
                    if (reTryCount >= RETRY_MAX)
                    {
                        message = string.Format("データベース：データテーブルの取得に失敗しました、リトライオーバーしました。(Exception) Message:[{0}]", ex.Message);
                        this.WriteProcessLogWithMessageBox(message);
                        throw new Exception(message, ex);
                    }
                    else if (reTryCount < 1)
                    {   //リトライ時は1回だけロギング
                        message = string.Format("データベース：データテーブルの取得に失敗しました、リトライします。(Exception) Message:[{0}]", ex.Message);
                        this.WriteProcessLog(message);
                    }
                }
                System.Threading.Thread.Sleep(RETRY_INTERVAL);
                this.Open(forceOpen: true);
                command.Connection = this.Connection;   //コネクションを破棄して再生しているので再割付が必要
            }
            message = "データベース：データテーブルの取得に失敗しました。";
            this.WriteProcessLog(message);
            throw new Exception(message);
        }
        /// <summary>
        /// データセットの取得
        /// </summary>
        /// <param name="command"></param>
        /// <returns>DataSetオブジェクト</returns>
        /// <remarks></remarks>
        public DataSet GetDataSet(SqlCommand command, bool useTransaction = false)
        {
            string message;
            var aDataSet = new DataSet();
            for (var reTryCount = 0; reTryCount <= RETRY_MAX; reTryCount++)
            {
                try
                {
                    if ((this.ConnectState == DbConnectState.OpenedWithTrans) || (this.ConnectState == DbConnectState.Opened))
                    {
                        using (SqlDataAdapter sqlDa = new SqlDataAdapter(command))
                        {
                            this.WriteProcessLog(this.GetStartLogText(command));
                            sqlDa.Fill(aDataSet);
                            foreach (DataTable t in aDataSet.Tables)
                            {
                                this.WriteProcessLog(this.GetEndLogText(command, t));
                            }
                        }
                        return aDataSet;
                    }
                    else
                    {
                        //throw new Exception("DBが接続されていないか、開いていません。");
                    }
                }
                catch (SqlException ex)
                {
                    if (reTryCount >= RETRY_MAX)
                    {
                        message = string.Format("データベース：データセットの取得に失敗しました、リトライオーバーしました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                        this.WriteProcessLogWithMessageBox(message);
                        throw new Exception(message, ex);
                    }
                    else if (reTryCount < 1)
                    {   //リトライ時は1回だけロギング
                        message = string.Format("データベース：データセットの取得に失敗しました、リトライします。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                        this.WriteProcessLog(message);
                    }
                }
                catch (Exception ex)
                {
                    if (reTryCount >= RETRY_MAX)
                    {
                        message = string.Format("データベース：データセットの取得に失敗しました、リトライオーバーしました。(Exception) Message:[{0}]", ex.Message);
                        this.WriteProcessLogWithMessageBox(message);
                        throw new Exception(message, ex);
                    }
                    else if (reTryCount < 1)
                    {   //リトライ時は1回だけロギング
                        message = string.Format("データベース：データセットの取得に失敗しました、リトライします。(Exception) Message:[{0}]", ex.Message);
                        this.WriteProcessLog(message);
                    }
                }
                System.Threading.Thread.Sleep(RETRY_INTERVAL);
                this.Open(forceOpen: true);
                command.Connection = this.Connection;   //コネクションを破棄して再生しているので再割付が必要
            }
            message = "データベース：データセットの取得に失敗しました。";
            this.WriteProcessLog(message);
            throw new Exception(message);
        }
        ///// <summary>
        ///// データリーダーの取得1
        ///// SQL文字列よりDataReaderオブジェクトを取得する。
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="useTransaction"></param>
        ///// <returns>SqlDataReaderオブジェクト</returns>
        ///// <remarks></remarks>
        //public SqlDataReader GetDataReader(string sql, bool useTransaction = false)
        //{
        //    SqlDataReader ret = default(SqlDataReader);
        //    if ((this.ConnectState == DbConnectState.OpenedWithTrans) || (this.ConnectState == DbConnectState.Opened))
        //    {
        //        try
        //        {
        //            SqlCommand command = (SqlCommand)this.GetCommand(useTransaction);
        //            command.CommandText = sql;
        //            command.CommandTimeout = this._commandTimeout;
        //            this.WriteProcessLog(this.GetStartLogText(command));
        //            ret = command.ExecuteReader();
        //            this.WriteProcessLog(this.GetEndLogText(command, ret));
        //            return ret;
        //        }
        //        catch (SqlException ex)
        //        {
        //            string message = string.Format("データベース：データリーダーの取得に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
        //            this.WriteProcessLog(message);
        //            throw new Exception(message, ex);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("DBが接続されていないか、開いていません。");
        //    }
        //}
        ///// <summary>
        ///// データリーダーの取得2
        ///// CommandオブジェクトよりDataReaderオブジェクトを取得する。
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="useTransaction"></param>
        ///// <returns>SqlDataReaderオブジェクト</returns>
        ///// <remarks></remarks>
        //public SqlDataReader GetDataReader(SqlCommand command, bool useTransaction = false)
        //{
        //    SqlDataReader ret = default(SqlDataReader);
        //    if ((this.ConnectState == DbConnectState.OpenedWithTrans) || (this.ConnectState == DbConnectState.Opened))
        //    {
        //        try
        //        {
        //            this.WriteProcessLog(this.GetStartLogText(command));
        //            ret = command.ExecuteReader();
        //            this.WriteProcessLog(this.GetEndLogText(command, ret));
        //            return ret;
        //        }
        //        catch (SqlException ex)
        //        {
        //            string message = string.Format("データベース：データリーダーの取得に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
        //            this.WriteProcessLog(message);
        //            throw new Exception(message, ex);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("DBが接続されていないか、開いていません。");
        //    }
        //}
        ///// <summary>
        ///// SQLを実行する
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="useTransaction"></param>
        ///// <returns>影響を受けた行数。-1はエラー。</returns>
        ///// <remarks></remarks>
        //public int ExecuteSQL(string sql, bool useTransaction = false)
        //{
        //    int ret = 0;
        //    if ((this.ConnectState == DbConnectState.OpenedWithTrans) || (this.ConnectState == DbConnectState.Opened))
        //    {
        //        try 
        //        {
        //            SqlCommand command = (SqlCommand)this.GetCommand(useTransaction);
        //            command.CommandText = sql;
        //            command.CommandTimeout = this._commandTimeout;
        //            this.WriteProcessLog(this.GetStartLogText(command));
        //            ret = command.ExecuteNonQuery();
        //            this.WriteProcessLog(this.GetEndLogText(command, ret));
        //            return ret;
        //        }
        //        catch (SqlException ex)
        //        {
        //            string message = string.Format("データベース：コマンドの実行に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
        //            this.WriteProcessLog(message);
        //            throw new Exception(message, ex);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("DBが接続されていないか、開いていません。");
        //    }
        //}
        /// <summary>
        /// SQLを実行する
        /// </summary>
        /// <param name="command"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ExecuteSQL(SqlCommand command, bool useTransaction = false)
        {   //★トランザクションの一部になってる可能性があるからリトライは個別には難しいね。
            int ret = 0;
            if ((this._connection != null) && (this._connection.State != ConnectionState.Closed))
            {
                try
                {
                    this.WriteProcessLog(this.GetStartLogText(command));
                    ret = command.ExecuteNonQuery();
                    this.WriteProcessLog(this.GetEndLogText(command, ret));
                    return ret;
                }
                catch (SqlException ex)
                {
                    string message = string.Format("データベース：コマンドの実行に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                    this.WriteProcessLog(message);
                    throw new Exception(message, ex);
                }
            }
            else
            {
                throw new Exception("DBが接続されていないか、開いていません。");
            }
        }
        ///// <summary>
        ///// ストアドプロシージャを実行する
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="useTransaction"></param>
        ///// <returns>影響を受けた行数。-1はエラー。</returns>
        ///// <remarks></remarks>
        //public int ExecuteStored(string sql, bool useTransaction = false)
        //{
        //    int ret = 0;
        //    if ((this.ConnectState == DbConnectState.OpenedWithTrans) || (this.ConnectState == DbConnectState.Opened))
        //    {
        //        try
        //        {
        //            SqlCommand command = (SqlCommand)this.GetCommand(useTransaction);
        //            command.CommandText = sql;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandTimeout = this._commandTimeout;
        //            this.WriteProcessLog(this.GetStartLogText(command));
        //            ret = command.ExecuteNonQuery();
        //            this.WriteProcessLog(this.GetEndLogText(command, ret));
        //            return ret;
        //        }
        //        catch (SqlException ex)
        //        {
        //            string message = string.Format("データベース：ストアドプロシジャの実行に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
        //            this.WriteProcessLog(message);
        //            throw new Exception(message, ex);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("DBが接続されていないか、開いていません。");
        //    }
        //}
        /// <summary>
        /// 接続DBのトランザクションオブジェクトを定義する
        /// </summary>
        /// <returns>処理結果ステータス</returns>
        /// <remarks></remarks>
        public DbResultState BeginTrans()
        {
            if (this.ConnectState != DbConnectState.OpenedWithTrans)
            {
                try
                {
                    this._transaction = this._connection.BeginTransaction();
                    return DbResultState.Done;
                }
                catch (SqlException ex)
                {
                    string message = string.Format("データベース：トランザクションの開始に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                    this.WriteProcessLog(message);
                    throw new Exception(message, ex);
                }
            }
            else
            {
                throw new Exception("トランザクションは既に開始しているか、接続できません。");
            }
        }
        /// <summary>
        /// トランザクション開始
        /// </summary>
        /// <param name="iso">接続トランザクションのロック動作</param>
        /// <returns>処理結果ステータス</returns>
        /// <remarks></remarks>
        public DbResultState BeginTrans(IsolationLevel iso)
        {
            if (this.ConnectState != DbConnectState.OpenedWithTrans)
            {
                try
                {
                    this._transaction = this._connection.BeginTransaction(iso);
                    return DbResultState.Done;
                }
                catch (SqlException ex)
                {
                    string message = string.Format("データベース：トランザクションの開始に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                    this.WriteProcessLog(message);
                    throw new Exception(message, ex);
                }
            }
            else
            {
                throw new Exception("トランザクションは既に開始しているか、接続できません。");
            }
        }
        /// <summary>
        /// トランザクションのロールバック
        /// </summary>
        /// <returns>処理結果ステータス</returns>
        /// <remarks></remarks>
        public DbResultState RollbackTrans()
        {
            if (this.ConnectState == DbConnectState.OpenedWithTrans)
            {
                try
                {
                    this._transaction.Rollback();
                    this._transaction = null;
                    return DbResultState.Done;
                }
                catch (SqlException ex)
                {
                    string message = string.Format("データベース：トランザクションのロールバックに失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                    this.WriteProcessLog(message);
                    throw new Exception(message, ex);
                }
            }
            else
            {
                throw new Exception("トランザクションが開始していません。");
            }
        }
        /// <summary>
        /// トランザクションのコミット
        /// </summary>
        /// <returns>処理結果ステータス</returns>
        /// <remarks></remarks>
        public DbResultState CommitTrans()
        {
            if ((this.ConnectState == DbConnectState.OpenedWithTrans))
            {
                try
                {
                    this._transaction.Commit();
                    this._transaction = null;
                    return DbResultState.Done;
                }
                catch (SqlException ex)
                {
                    string message = string.Format("データベース：トランザクションのコミットに失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                    this.WriteProcessLog(message);
                    throw new Exception(message, ex);
                }
            }
            else
            {
                throw new Exception("トランザクションが開始していません。");
            }
        }
        /// <summary>
        /// コマンドオブジェクトの取得
        /// </summary>
        /// <param name="useTransaction">トランザクションを使用するかどうか</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DbCommand GetCommand(bool useTransaction = false)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                if ((this.ConnectState != DbConnectState.NoObject))
                {
                    command.CommandText = "";
                    command.Connection = _connection;
                    command.CommandTimeout = this._commandTimeout;
                    if ((useTransaction))
                    {
                        if ((this.ConnectState != DbConnectState.OpenedWithTrans))
                        {
                            throw new Exception("トランザクションが開始していません。");
                        }
                        else
                        {
                            command.Transaction = this._transaction;
                        }
                    }
                }
                else
                {
                    throw new Exception("接続オブジェクトがありません。");
                }
                return command;
            }
            catch (SqlException ex)
            {
                string message = string.Format("データベース：コマンドオブジェクトの取得に失敗しました。(SqlException) Message:[{0}]/ErrorNo:[{1}]", ex.Message, ex.Number);
                this.WriteProcessLog(message);
                throw new Exception(message, ex);
            }
        }
        /// <summary>
        /// DbCommandオブジェクトよりパラメータを生成
        /// </summary>
        /// <param name="command">パラメータの生成元のDbCommand</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="value">パラメータ値</param>
        /// <returns>生成されたパラメータ</returns>
        /// <remarks></remarks>
        public DbParameter GetCmdParameter(DbCommand command, string parameterName, string value)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            return parameter;
        }
        /// <summary>
        /// レコード件数を取得する
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private int GetRecordCount(DataTable dt)
        {
            int count = 0;
            count = 0;
            if ((dt != null) && dt.Rows.Count > 0)
            {
                count = dt.Rows.Count;
            }
            return count;
        }
        /// <summary>
        /// 開始ログ出力
        /// </summary>
        /// <param name="command">SqlCommand</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetStartLogText(SqlCommand command)
        {
            string ret = null;
            ret = "";
            ret = "DB処理開始" + Environment.NewLine;
            switch (command.CommandType)
            {
                case CommandType.Text:
                    // SQL
                    ret += "処理SQL：[ " + command.CommandText + " ]" + Environment.NewLine;
                    break;
                case CommandType.StoredProcedure:
                    // ストアド
                    ret += "処理ストアド：[ " + command.CommandText + " ]" + Environment.NewLine;
                    ret += "引数：";

                    if (command.Parameters.Count > 0)
                    {
                        foreach (SqlParameter arg in command.Parameters)
                        {
                            ret += arg.ParameterName + " = " + arg.Value + Environment.NewLine;
                        }
                    }
                    else
                    {
                        ret += "無し" + Environment.NewLine;
                    }
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 結果ログ出力（Insert／Update／Delete）
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="redCode">SQL返却値</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetEndLogText(SqlCommand command, int redCode)
        {
            string ret = null;
            ret = "";
            ret = "DB処理完了" + Environment.NewLine;
            switch (command.CommandType)
            {
                case CommandType.Text:
                    // SQL
                    ret += "処理SQL：[ " + command.CommandText + " ]" + Environment.NewLine;
                    break;
                case CommandType.StoredProcedure:
                    // ストアド
                    ret += "処理ストアド：[ " + command.CommandText + " ]" + Environment.NewLine;
                    break;
                default:
                    return ret;
            }

            if (redCode < 0)
            {
                ret += "処理結果：失敗" + Environment.NewLine;
            }
            else
            {
                ret += "処理結果：" + redCode.ToString() + " 件 処理しました。" + Environment.NewLine;
            }
            return ret;
        }
        /// <summary>
        /// 結果ログ出力（Select）
        /// </summary>
        /// <param name="command">SqlCommand</param>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetEndLogText(SqlCommand command, DataTable dt)
        {
            string ret = null;
            ret = "";
            ret = "DB処理完了" + Environment.NewLine;
            switch (command.CommandType)
            {
                case CommandType.Text:
                    // SQL
                    ret += "処理SQL：[ " + command.CommandText + " ]" + Environment.NewLine;
                    break;
                case CommandType.StoredProcedure:
                    // ストアド
                    ret += "処理ストアド：[ " + command.CommandText + " ]" + Environment.NewLine;
                    break;
                default:
                    return ret;
            }
            ret += "取得件数：" + GetRecordCount(dt).ToString() + " 件" + Environment.NewLine;
            return ret;
        }
        /// <summary>
        /// 結果ログ出力（Select）
        /// </summary>
        /// <param name="command">SqlCommand</param>
        /// <param name="reader">SqlDataReader</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetEndLogText(SqlCommand command, SqlDataReader reader)
        {
            string ret = null;
            ret = "";
            ret = "DB処理完了" + Environment.NewLine;
            switch (command.CommandType)
            {
                case CommandType.Text:
                    // SQL
                    ret += "処理SQL：[ " + command.CommandText + " ]" + Environment.NewLine;
                    break;
                case CommandType.StoredProcedure:
                    // ストアド
                    ret += "処理ストアド：[ " + command.CommandText + " ]" + Environment.NewLine;
                    break;
                default:
                    return ret;
            }
            ret += "取得結果：";
            if (reader.HasRows)
            {
                ret += "成功" + Environment.NewLine;
            }
            else
            {
                ret += "失敗" + Environment.NewLine;
            }
            return ret;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検知するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    this.Close();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~Test() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
