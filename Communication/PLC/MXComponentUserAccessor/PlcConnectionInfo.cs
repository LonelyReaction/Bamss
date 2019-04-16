namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC接続情報保持クラス
    /// </summary>
    public class PlcConnectionInfo
    {
        private string _name;
        private int _atcLogicalNo;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">接続名</param>
        /// <param name="actLogicalNo">接続論理番号</param>
        public PlcConnectionInfo(string name, int actLogicalNo)
        {
            this._name = name;
            this._atcLogicalNo = actLogicalNo;
        }
        /// <summary>
        /// 接続名取得
        /// </summary>
        public string Name
        {
            get { return this._name; }
        }
        /// <summary>
        /// 接続論理番号取得
        /// </summary>
        public int ActLogicalNo
        {
            get { return this._atcLogicalNo; }
        }
    }
}
