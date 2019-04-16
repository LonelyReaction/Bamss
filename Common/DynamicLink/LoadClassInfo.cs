using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMSS.Common.DynamicLink
{
    /// <summary>
    /// クラスロード情報クラス
    /// </summary>
    public class LoadClassInfo
    {
        /// <summary>
        /// アセンブリ名
        /// </summary>
        public string AssemblyFileName { get; private set; }
        /// <summary>
        /// クラス名（完全名）
        /// </summary>
        public string ClassName { get; private set; }
        /// <summary>
        /// コンストラクタへのパラメータ
        /// </summary>
        public object[] Args { get; private set; }
        /// <summary>
        /// コンストラクタ（完全クラス名指定）
        /// </summary>
        /// <param name="assemblyFileName">アセンブリ名</param>
        /// <param name="classFullName">完全クラス名</param>
        /// <param name="args">コンストラクタへのパラメータ</param>
        public LoadClassInfo(string assemblyFileName, string classFullName, object[] args = null)
        {
            this.AssemblyFileName = assemblyFileName;
            this.ClassName = classFullName;
            this.Args = args;
        }
        /// <summary>
        /// コンストラクタ（名前空間／クラス名個別指定）
        /// </summary>
        /// <param name="assemblyFileName">アセンブリ名</param>
        /// <param name="nameSpace">名前空間</param>
        /// <param name="className">クラス名（名前空間を含まない）</param>
        /// <param name="args">コンストラクタへのパラメータ</param>
        public LoadClassInfo(string assemblyFileName, string nameSpace, string className, object[] args = null)
        {
            this.AssemblyFileName = assemblyFileName;
            this.ClassName = string.Format("{0}.{1}", nameSpace, className);
            this.Args = args;
        }
    }
}
