using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BAMSS.Data
{
    /// <summary>
    /// データベースレコード関連拡張クラス
    /// </summary>
    public static class RecordsExtendedMethods
    {
        /// <summary>
        /// "List<T>"からDataTable(データテーブル)を生成するメソッド
        /// 要素クラスは、RecordBaseを基底クラスに持ち、引数のないコンストラクタを備えるクラスが対象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable BuildDataTable<T>(this IList<T> list) where T : RecordBase, new()
        {
            var dt = (new T()).InitializeDataTable();
            foreach (var item in list) { var dr = item.ToDataRow(dt); }
            return dt;
        }
        /// <summary>
        /// データテーブルからListを生成
        /// </summary>
        /// <typeparam name="T">RecordBaseの派生クラス</typeparam>
        /// <param name="dt">データテーブル</param>
        /// <returns>T型のList</returns>
        public static IList<T> BuildList<T>(this DataTable dt) where T : RecordBase, new()
        {
            var list = new List<T>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var item = new T();
                item.BuildRecord(dt.Rows[i]);
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// パラメータセット
        /// </summary>
        /// <param name="paramTarget">SqlParameterCollection</param>
        /// <param name="paramName">パラメータ名</param>
        /// <param name="typeName">タイプ名</param>
        /// <param name="list">データリスト</param>
        public static void SetListParam(this SqlParameterCollection paramTarget, string paramName, string typeName, IList<RecordBase> list)
        {
            paramTarget.Add(paramName, SqlDbType.Structured);
            paramTarget[paramName].Direction = ParameterDirection.Input;
            paramTarget[paramName].TypeName = typeName;
            paramTarget[paramName].Value = list.BuildDataTable();
        }
    }
}
