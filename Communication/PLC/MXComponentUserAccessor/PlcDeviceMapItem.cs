namespace BAMSS.MXComponent
{
    /// <summary>
    /// マップアイテム（データ項目）情報クラス
    /// </summary>
    public class PlcDeviceMapItem
    {
        private string _name;
        private string _address;
        private int _size;
        private PlcFieldType _type;
        private int _decimalLength;
        private bool _unsigned;
        private string _value;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">データ名</param>
        /// <param name="address">相対アドレス（マップの先頭ワードを0としたアドレス）</param>
        /// <param name="size">データ項目サイズ（ASCIIの場合8bitで1／BINARYの場合16bitで1／BCDの場合4bitで1）</param>
        /// <param name="type">データ属性（ASCII/BINARY/BCD）</param>
        /// <param name="decimalLength">小数点桁数</param>
        /// <param name="unsigned">正数のみの場合:True／負数も扱う場合:False</param>
        public PlcDeviceMapItem(string name, string address, int size, PlcFieldType type, int decimalLength = 0, bool unsigned = true)
        {
            this._name = name;
            this._address = address;
            this._size = size;
            this._type = type;
            this._decimalLength = decimalLength;
            this._unsigned = unsigned;
            this._value = "";
        }
        /// <summary>
        /// 値の取得
        /// </summary>
        public string Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        /// <summary>
        /// データ項目名称の取得
        /// </summary>
        public string Name
        {
            get { return this._name; }
        }
        /// <summary>
        /// アドレスの取得
        /// </summary>
        public string Address
        {
            get { return this._address; }
        }
        /// <summary>
        /// データサイズの取得
        /// </summary>
        public int Size
        {
            get { return this._size; }
        }
        /// <summary>
        /// 小数点桁数の取得
        /// </summary>
        public int DecimalLength
        {
            get { return this._decimalLength; }
        }
        /// <summary>
        /// 数値が正数のみを扱うか否かの取得
        /// </summary>
        public bool UnSigned
        {
            get { return this._unsigned; }
        }
        /// <summary>
        /// データ属性の取得
        /// </summary>
        public PlcFieldType Type
        {
            get { return this._type; }
        }
    }
}
