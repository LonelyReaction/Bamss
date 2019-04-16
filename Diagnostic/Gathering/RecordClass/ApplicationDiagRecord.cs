using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMSS.Diagnostic
{
    /// <summary>
    /// ApplicationDiagnosticクラスにて情報収集を行う際に、インラインで収集処理を設定する為のクラス。
    /// </summary>
    public class ApplicationDiagRecord : IApplicationDiagRecord
    {
        private string _header;
        private Func<string> _recBuilder;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="header">ヘッダーレコード文字列</param>
        /// <param name="predicate">データレコードを生成するデリゲート</param>
        public ApplicationDiagRecord(string header, Func<string> predicate)
        {
            this._header = header;
            this._recBuilder = predicate;
        }
        /// <summary>
        /// ヘッダー文字列取得
        /// </summary>
        /// <returns></returns>
        public string Header()
        {
            return this._header;
        }
        /// <summary>
        /// データレコード文字列取得
        /// </summary>
        /// <returns></returns>
        public string Record()
        {
            if (this._recBuilder == null) return "";
            else return this._recBuilder.Invoke();
        }
    }
}
