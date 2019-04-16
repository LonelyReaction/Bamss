using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace BAMSS.Data
{
    /// <summary>
    /// データベースレコードテーブルの基底クラス
    /// </summary>
    public class RecordBase
    {
        /// <summary>
        /// DataTable初期化処理
        /// </summary>
        /// <returns></returns>
        public DataTable InitializeDataTable()
        {
            var dt = new DataTable();
            var members = this.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (MemberInfo m in members.Where(c => c.MemberType == MemberTypes.Property)) 
            {
                dt.Columns.Add(m.Name, Type.GetType(this.NullableConvert(this.GetType().GetProperty(m.Name).PropertyType.FullName))); 
            }
            return dt;
        }
        /// <summary>
        /// DataSetがNullableに対応していないので、Nullable型を非Nullalbe型に変換する関数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string NullableConvert(string value)
        {
            if (value.Contains("System.Nullable")) 
            {   //null許容型への対応
                if (value.Contains(typeof(System.DateTime).ToString())) return typeof(System.DateTime).ToString();
                if (value.Contains(typeof(System.Int16).ToString())) return typeof(System.Int16).ToString();
                if (value.Contains(typeof(System.Int32).ToString())) return typeof(System.Int32).ToString();
                if (value.Contains(typeof(System.Int64).ToString())) return typeof(System.Int64).ToString();
                if (value.Contains(typeof(System.Double).ToString())) return typeof(System.Double).ToString();
                if (value.Contains(typeof(System.Decimal).ToString())) return typeof(System.Decimal).ToString();
            }
            return value;
        }

        #region DataRow
            /// <summary>
            /// DataTableに自レコードをDataRow化して追加する
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public DataRow ToDataRow(DataTable dt)
            {
                var dr = dt.NewRow();
                var members = this.GetType().GetMembers();
                foreach (MemberInfo m in members.Where(c => c.MemberType == MemberTypes.Property))
                {   //処理対象＝プロパテェだけ
                    try
                    {
                        var property = this.GetType().GetProperty(m.Name);
                        if (property.CanRead)
                        {   //読込可能プロパテェのみ処理
                            var value = property.GetValue(this, null);
                            if (value == null)
                                dr[m.Name] = DBNull.Value;
                            else
                                dr[m.Name] = value;
                        }
                    }
                    catch { /*ドンマイエラー*/ }
                }
                dt.Rows.Add(dr);
                return dr;
            }
            /// <summary>
            /// DataRowから自レコードを生成する
            /// </summary>
            /// <param name="value"></param>
            public void BuildRecord(DataRow value)
            {
                var members = this.GetType().GetMembers();
                foreach (MemberInfo m in members.Where(c => c.MemberType == MemberTypes.Property))
                {   //処理対象＝プロパテェだけ
                    try
                    {
                        var property = this.GetType().GetProperty(m.Name);
                        if (property.CanWrite)
                        {   //書込可能プロパテェのみ処理
                            if (value[m.Name] == DBNull.Value)
                                property.SetValue(this, null, null);
                            else
                                property.SetValue(this, value[m.Name], null);
                        }
                    }
                    catch { /*ドンマイエラー*/ }
                }
            }
        #endregion

        #region CSV
            /// <summary>
            /// 自レコードからCSV形式レコード文字列を出力する
            /// </summary>
            /// <param name="delimiter">区切り文字</param>
            /// <param name="terminator">レコード終端文字(列)</param>
            /// <returns></returns>
            public string ToCSV(char delimiter = ',', string terminator = "\r\n")
            {
                var members = this.GetType().GetMembers();
                var csv = string.Empty;
                foreach (MemberInfo m in members.Where(c => c.MemberType == MemberTypes.Property))
                {   //処理対象＝プロパテェだけ
                    try
                    {
                        var property = this.GetType().GetProperty(m.Name);
                        object outValue = null;
                        csv += string.Format("{0}{1}", (csv == string.Empty) ? string.Empty : delimiter.ToString(), property.GetValue(outValue));
                    }
                    catch { /*ドンマイエラー*/ }
                }
                return string.Format("{0}{1}", csv, terminator);
            }
            /// <summary>
            /// CSV形式レコード文字列から自レコードを生成する
            /// </summary>
            /// <param name="value">区切り形式データ</param>
            /// <param name="delimiter">区切り文字</param>
            public void BuildRecord(string value, char delimiter = ',')
            {
                var members = this.GetType().GetMembers();
                var items = value.Split(delimiter);
                var index = 0;
                foreach (MemberInfo m in members.Where(c => c.MemberType == MemberTypes.Property))
                {   //処理対象＝プロパテェだけ
                    try
                    {
                        var property = this.GetType().GetProperty(m.Name);
                        if (property.CanWrite)
                        {   //書込可能プロパテェのみ処理
                            property.SetValue(this, items[index], null);
                        }
                        index++;
                        if (items.Count() > index) break;
                    }
                    catch { /*ドンマイエラー*/ }
                }
            }
        #endregion
    }
}
