using System.Collections.Generic;
using System.Reflection;
using BAMSS.Common.DynamicLink;

namespace BAMSS.UI.SwapPage
{
    /// <summary>
    /// SwapPageのページ構成を生成するクラス（SwapPageBase型のジェネリック）
    /// </summary>
    /// <typeparam name="T">where T : SwapPageBase</typeparam>
    public class SwapPageScenarioBuilder<T> where T : SwapPageBase
    {
        private List<T> _list = new List<T>();
        /// <summary>
        /// ページ追加（クラス情報クラス指定）
        /// </summary>
        /// <param name="classInfo">クラス情報</param>
        public void AddClass(LoadClassInfo classInfo)
        {
            var asm = Assembly.LoadFrom(classInfo.AssemblyFileName);
            var inst = asm.CreateInstance(classInfo.ClassName, false, BindingFlags.CreateInstance, null, classInfo.Args, null, null);
            this._list.Add((T)inst);
        }
        /// <summary>
        /// ページ追加（各要素指定1）
        /// </summary>
        /// <param name="assemblyFileName">アセンブリ名</param>
        /// <param name="classFullName">クラス完全名</param>
        /// <param name="args">コンストラクタパラメータ</param>
        public void AddClass(string assemblyFileName, string classFullName, object[] args)
        {
            var asm = Assembly.LoadFrom(assemblyFileName);
            var inst = asm.CreateInstance(classFullName, false, BindingFlags.CreateInstance, null, args, null, null);
            this._list.Add((T)inst);
        }
        /// <summary>
        /// ページ追加（各要素指定2）
        /// </summary>
        /// <param name="assemblyFileName">アセンブリ名</param>
        /// <param name="nameSpace">名前空間</param>
        /// <param name="className">クラス名（名前空間を含めない）</param>
        /// <param name="args">コンストラクタパラメータ</param>
        public void AddClass(string assemblyFileName, string nameSpace, string className, object[] args)
        {
            var asm = Assembly.LoadFrom(assemblyFileName);
            var inst = asm.CreateInstance(string.Format("{0}.{1}", nameSpace, className), false, BindingFlags.CreateInstance, null, args, null, null);
            this._list.Add((T)inst);
        }
        /// <summary>
        /// SwapPage構造リスト取得
        /// </summary>
        /// <returns></returns>
        public List<T> SwapPageList() { return this._list; }
    }
}
