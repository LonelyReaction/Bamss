using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC機器のプロキシオブジェクトとなるクラス。
    /// </summary>
    /// <remarks>
    /// 2013.08.21 Wed BAMSS Jun.Yokoe
    /// MXComponentのステーション番号が再オープン時に0に戻る不具合修正
    /// </remarks>
    public class MXComponentCoreAccessor
    {
		    //MX Component
	    private ACTMULTILib.ActEasyIF _plc = null;
	    private MXComponentCoreBitValueReader _plcBitReader = null;
	    private int _actLogicalStationNumber = 0;

        private static Dictionary<int, MXComponentCoreAccessor> _instaces = new Dictionary<int, MXComponentCoreAccessor>();
        public static MXComponentCoreAccessor GetInstance(int actLogicalStationNumber)
	    {
		    if ((_instaces.ContainsKey(actLogicalStationNumber))) {
			    return _instaces[actLogicalStationNumber];
		    } else {
                MXComponentCoreAccessor newInstance = new MXComponentCoreAccessor(actLogicalStationNumber);
			    _instaces.Add(actLogicalStationNumber, newInstance);
			    return newInstance;
		    }
	    }

	    /// <summary>
	    /// MXComponentインスタンスを生成し、コンストラクタパラメータの論理番号にて初期化する。
	    /// </summary>
	    /// <remarks></remarks>
	    private void CreateMXComponent(int actLogicalStationNumber)
	    {
		    this._plc = new ACTMULTILib.ActEasyIF { ActLogicalStationNumber = this._actLogicalStationNumber };
	    }

	    /// <summary>
	    /// 既定のコンストラクタ。
	    /// 設定番号を指定し、それによって特定される PLC 機器のプロキシオブジェクトを生成する。
	    /// </summary>
	    /// <param name="actLogicalStationNumber"></param>
	    /// <remarks></remarks>
        private MXComponentCoreAccessor(int actLogicalStationNumber)
	    {
		    this._actLogicalStationNumber = actLogicalStationNumber;
		    this.CreateMXComponent(actLogicalStationNumber);
	    }

	    /// <summary>
	    /// PLC要求応答取得
	    /// </summary>
	    /// <value></value>
	    /// <returns></returns>
	    /// <remarks></remarks>
	    public MXComponentCoreBitValueReader PlcBitReader {
		    get { return this._plcBitReader; }
		    set { this._plcBitReader = value; }
	    }

	    /// <summary>
	    /// PLC機器との通信を接続する。
	    /// </summary>
	    /// <remarks></remarks>
	    public void Open()
	    {
		    if ((this._plc == null)) {
			    this.CreateMXComponent(this._actLogicalStationNumber);
		    }

		    if ((this._plc.Open() != 0)) {
			    throw new IOException("PLCデバイスへの接続に失敗しました。");
		    }
	    }

	    /// <summary>
	    /// PLC機器との通信を切断する。
	    /// </summary>
	    /// <remarks></remarks>
	    public void Close()
	    {
		    if ((this._plc.Close() != 0)) {
			    throw new IOException("PLCデバイスとの切断に失敗しました。");
		    }

		    this._plc = null;
	    }

	    /// <summary>
	    /// 通信の切断を試みたあと、再接続を行う。
	    /// 接続成功時は true を、失敗時は false を返す。
	    /// </summary>
	    /// <returns></returns>
	    /// <remarks></remarks>
	    private bool RetryOpen()
	    {
		    try {
			    this.Close();
		    } catch {
		    }
		    try {
			    this.Open();
			    return true;
		    } catch {
			    return false;
		    }
	    }

	    /// <summary>
	    /// ビットデバイス読み込み処理
	    /// </summary>
	    /// <param name="address">読み込むビットアドレス</param>
	    /// <remarks></remarks>
	    public bool ReadBit(string address)
	    {
		    int readValue = 0;

		    bool isSuccess = false;
		    for (int i = 0; i <= 1; i++) {
			    if (((i == 0) || this.RetryOpen())) {
				    try {
					    isSuccess = (this._plc.ReadDeviceRandom(address, 1, out readValue) == 0);
				    } catch {
				    }
                    if (isSuccess) break;
                }
		    }
		    if (!isSuccess) {
			    throw new IOException(string.Format("単一ビットデバイスからの読み込みに失敗しました。Address:[{0}]", address));
		    }

		    return (readValue == 1);
	    }

	    /// <summary>
	    /// ビットデバイス複数読み込み処理
	    /// </summary>
	    /// <param name="addressList">読み込むビットアドレスリスト</param>
	    /// <remarks></remarks>
	    public List<bool> ReadBit(List<string> addressList)
	    {
		    int[] readValue = null;
		    string addresses = "";

		    for (int addressNo = 0; addressNo <= (addressList.Count - 1); addressNo += 1) {
			    addresses += addressList[addressNo];
			    if ((addressNo < (addressList.Count - 1))) {
				    addresses += "\n";
			    }
		    }
		    readValue = new int[addressList.Count];

		    bool isSuccess = false;
		    for (int i = 0; i <= 1; i++) {
			    if (((i == 0) || this.RetryOpen())) {
				    try {
					    isSuccess = (this._plc.ReadDeviceRandom(addresses, addressList.Count, out readValue[0]) == 0);
				    } catch {
				    }
                    if (isSuccess) break;
			    }
		    }
		    if (!isSuccess) {
			    throw new IOException(string.Format("複数ビットデバイスからの読み込みに失敗しました。Address:[{0}]", addresses));
		    }

            var list = new List<int>();
            list.AddRange(readValue);
		    return list.ConvertAll<bool>(i => i > 0);
	    }

	    /// <summary>
	    /// ビットデバイス書き込み処理
	    /// </summary>
	    /// <param name="address">書き込むビットアドレス</param>
	    /// <param name="value"></param>
	    /// <remarks></remarks>
	    public void WriteBit(string address, bool value)
	    {
		    bool isSuccess = false;
		    for (int i = 0; i <= 1; i++) {
			    if (((i == 0) || this.RetryOpen())) {
				    try {
                        int[] data = {(value ? 1 : 0)};
					    isSuccess = (this._plc.WriteDeviceRandom(address, 1, ref data[0]) == 0);
				    } catch {
				    }
                    if (isSuccess) break;
                }
		    }
		    if (!isSuccess) {
			    throw new IOException(string.Format("単一ビットデバイスへの書き込みに失敗しました。Address:[{0}]", address));
		    }
	    }

	    /// <summary>
	    /// ビットデバイス書き込み処理
	    /// </summary>
	    /// <param name="address">書き込むビットアドレス</param>
	    /// <param name="value"></param>
	    /// <remarks></remarks>
	    public void WriteBit(string address, int value)
	    {
		    bool isSuccess = false;
		    for (int i = 0; i <= 1; i++) {
			    if (((i == 0) || this.RetryOpen())) {
				    try {
                        int[] data = { value };
                        isSuccess = (this._plc.WriteDeviceRandom(address, 1, ref data[0]) == 0);
				    } catch {
				    }
                    if (isSuccess) break;
                }
		    }
		    if (!isSuccess) {
			    throw new IOException(string.Format("単一ビットデバイスへの書き込みに失敗しました。Address:[{0}]", address));
		    }
	    }

	    /// <summary>
	    /// ビットデバイス複数書込み処理
	    /// </summary>
	    /// <param name="addressList">書き込むビットアドレスリスト</param>
	    /// <remarks></remarks>
	    public void WriteBit(List<string> addressList, List<bool> valueList)
	    {
		    bool isSuccess = false;
		    string addresses = "";
		    for (int addressNo = 0; addressNo <= (addressList.Count - 1); addressNo += 1) {
			    addresses += addressList[addressNo];
			    if ((addressNo < (addressList.Count - 1))) {
				    addresses += "\n";
			    }
		    }
		    for (int i = 0; i <= 1; i++) {
			    if (((i == 0) || this.RetryOpen())) {
				    try {
                        isSuccess = (this._plc.WriteDeviceRandom(addresses, addressList.Count, ref (valueList.ConvertAll<int>(j => j ? 1 : 0).ToArray()[0])) == 0);
				    } catch {
				    }
                    if (isSuccess) break;
                }
		    }
		    if (!isSuccess) {
			    throw new IOException(string.Format("複数ビットデバイスへの書き込みに失敗しました。Address:[{0}]", addresses));
		    }
	    }

	    /// <summary>
	    /// データ読み込み処理。
	    /// 例外もしくはエラーの場合、一度だけ再接続してリトライを試みる。
	    /// 処理成功時は true を、失敗時は false を返す。
	    /// </summary>
	    /// <param name="address">読込先頭アドレス</param>
	    /// <param name="data">読込バッファ（予め読込サイズにしておく必要有り）</param>
	    /// <returns></returns>
	    /// <remarks></remarks>
	    public bool ReadWord(string address, ref int[] data)
	    {
		    for (int i = 0; i <= 1; i++) {
			    if (((i == 0) || this.RetryOpen())) {
                    try
                    {
                        if ((this._plc.ReadDeviceBlock(address, data.Length, out data[0]) == 0))
                        {
                            return true;
                        }
                    }
                    catch { }
			    }
		    }
		    throw new IOException(string.Format("PLCデバイスからの読み込みに失敗しました。Address:[{0}]/Size:[{1}]", address, data.Length));
	    }

	    /// <summary>
	    /// データ書き込み処理。
	    /// 例外もしくはエラーの場合、一度だけ再接続してリトライを試みる。
	    /// 処理成功時は true を、失敗時は false を返す。
	    /// </summary>
	    /// <param name="address">書込先頭アドレス</param>
	    /// <param name="data">書込データ（ワード単位の配列）</param>
	    /// <returns></returns>
	    /// <remarks></remarks>
	    public bool WriteWord(string address, int[] data)
	    {
		    for (int i = 0; i <= 1; i++) {
			    if (((i == 0) || this.RetryOpen())) {
				    try { if ((this._plc.WriteDeviceBlock(address, data.Length, ref data[0]) == 0)) return true; } 
                    catch { }
			    }
		    }
		    return false;
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="address"></param>
	    /// <returns></returns>
	    /// <remarks></remarks>
	    private string GetSendAddressForMXComponent(string address)
	    {
		    return "";
	    }

	    /// <summary>
	    /// 既に読込済みのワード配列を部分的にアスキー文字列として取得する。
	    /// </summary>
	    /// <param name="start">読込先頭インデックス</param>
	    /// <param name="length">読込ワード数</param>
	    /// <param name="data">読込元データ</param>
	    /// <returns></returns>
	    /// <remarks></remarks>
	    public string ConvWordAsAscii(int start, int length, int[] data)
	    {
		    int size = (int)((length + 1) / 2);

		    // Ascii項目の読込配列からAscii文字列(Sring)を取得する
		    string result = "";
		    int check = 0;

		    for (int i = start; i <= (start + size - 1); i++) {
			    //上位ビット
                check = (int)(data[i] / 256);
			    if ((check > 0)) {
                    result = result + (char)(check);
			    } else {
                    result = result + (char)(32);
			    }
			    //下位ビット
			    check = (data[i] % 256);
			    if ((check > 0)) {
                    result = result + (char)(check);
			    } else {
                    result = result + (char)(32);
			    }
		    }

            return result.Substring(0, length);
	    }

	    /// <summary>
	    /// 既に読込済みのワード配列を部分的にバイナリ値として文字列に取得する。
	    /// </summary>
	    /// <param name="start">読込先頭インデックス</param>
	    /// <param name="size">読込ワード数</param>
	    /// <param name="data">読込元データ</param>
	    /// <param name="hasMinus">マイナス値があり得るか否か</param>
	    /// <param name="decimalLength">少数桁数（整数時は0）</param>
	    /// <remarks></remarks>
	    public string ConvWordAsBinary(int start, int size, int[] data, bool hasMinus, int decimalLength)
	    {
		    int[] workData = new int[size];
		    if ((hasMinus)) 
            {
			    Array.ConstrainedCopy(data, start, workData, 0, size);
			    switch (size) 
                {
				    case 1:
                        {
                            ushort result = 0;
                            for (int i = (workData.Length - 1); i >= 0; i += -1)
                            {
                                result <<= 16;
                                result += (ushort)workData[i];
                            }
                            return Convert.ToString(Convert.ToInt16(Convert.ToString(result, 16), 16) / (Math.Pow(10, decimalLength)));
                        }
                    case 2:
                        {
                            uint result = 0;
                            for (int i = (workData.Length - 1); i >= 0; i += -1)
                            {
                                result <<= 16;
                                result += (uint)workData[i];
                            }
                            return Convert.ToString(Convert.ToInt32(Convert.ToString(result, 16), 16) / (Math.Pow(10, decimalLength)));
                        }
                    default:
                        //4
                        {
                            ulong result = 0;
                            for (int i = (workData.Length - 1); i >= 0; i += -1)
                            {
                                result <<= 16;
                                result += (ulong)workData[i];
                            }
                            return Convert.ToString(Convert.ToInt64(result.ToString("X"), 16) / (Math.Pow(10, decimalLength)));
                        }
			    }
		    } else {
			    Array.ConstrainedCopy(data, start, workData, 0, size);
			    switch (size) 
                {
                    case 1:
                        {
                            ushort result = 0;
                            for (int i = (workData.Length - 1); i >= 0; i += -1)
                            {
                                result <<= 16;
                                result += (ushort)workData[i];
                            }
                            return Convert.ToString(result / (Math.Pow(10, decimalLength)));
                        }
                    case 2:
                        {
                            uint result = 0;
                            for (int i = (workData.Length - 1); i >= 0; i += -1)
                            {
                                result <<= 16;
                                result += (uint)workData[i];
                            }
                            return Convert.ToString(result / (Math.Pow(10, decimalLength)));
                        }
                    default:
                        //4
                        {
                            ulong result = 0;
                            for (int i = (workData.Length - 1); i >= 0; i += -1)
                            {
                                result <<= 16;
                                result += (ulong)workData[i];
                            }
                            return Convert.ToString(result / (Math.Pow(10, decimalLength)));
                        }
			    }
		    }
	    }

	    /// <summary>
	    /// ワードデバイスBCD読み込み処理
	    /// </summary>
	    /// <param name="start">読込先頭インデックス</param>
	    /// <param name="length">読込文字数（１ワード＝４文字）</param>
	    /// <param name="data">読込元データ</param>
	    /// <remarks></remarks>
        public string ConvWordAsBcd(int start, int length, int[] data, int decimalLength)
	    {
            int size = (int)((length + 3) / 4);

		    // BCD項目の読込配列から数値(String)を取得する
		    string result = "";

		    for (int i = start; i <= (start + size - 1); i++) {
			    dynamic workValue = data[i];
			    dynamic shiftValue = 4096;
			    for (int j = 0; j <= 3; j++) {
                    result += (int)(workValue / shiftValue);
                    workValue -= Convert.ToInt32(result.Substring(((i - start) * 4) + j, 1)) * shiftValue;
                    shiftValue /= 16;
			    }
		    }
            return Convert.ToString(long.Parse(result.Substring(result.Length - length, length)) / (Math.Pow(10, decimalLength)));
	    }

	    /// <summary>
	    /// ASCII文字列→ワードデバイス配列
	    /// </summary>
	    /// <param name="value">ASCIIデータ</param>
	    /// <remarks></remarks>
	    public int[] ConvertAsciiAsWord(string value, int size)
	    {
		    if ((size != 0)) {
			    value = (value + (new string(' ', size))).Substring(0, size);
		    }
            int wordSize = (int)(value.Length / 2);
		    dynamic lengthIsOdd = ((value.Length % 2) > 0);
		    int[] data = new int[(lengthIsOdd ? wordSize : wordSize - 1) + 1];

		    // 文字列からAscii項目書込用配列を生成する
		    for (int i = 0; i <= (wordSize - 1); i++) {
                data[i] = ((int)(value.ToCharArray(i * 2, 1)[0]) * 256) + (int)(value.ToCharArray(i * 2 + 1, 1)[0]);
			    //上位下位の場合
			    //data(i) = (Asc(Mid(value, i * 2 + 2, 1)) * 256) + Asc(Mid(value, i * 2 + 1, 1))     '下位上位の場合
		    }
		    if ((lengthIsOdd)) {
			    data[wordSize] = ((int)(value.ToCharArray(wordSize * 2, 1)[0]) * 256) + 32;
			    //上位下位の場合
			    //data(wordSize) = (Asc(Mid(value, (wordSize) * 2 + 1, 1)))               '下位上位の場合
		    }
		    return data;
	    }

	    /// <summary>
	    /// ワードデバイスASCIIコード書き込み処理
	    /// </summary>
	    /// <param name="address">書込先頭アドレス</param>
	    /// <param name="value">書込データ</param>
	    /// <remarks></remarks>
	    public void WriteWordAsAscii(string address, string value, int size)
	    {
		    // データ書き込み
		    int[] data = this.ConvertAsciiAsWord(value, size);
		    if (!this.WriteWord(address, data)) {
			    throw new IOException(string.Format("PLCワードデバイスへのASCII書き込みに失敗しました。address:[{0}]/size:[{1}]", address, data.Length));
		    }
	    }

	    /// <summary>
	    /// BinaryHex文字列→ワードデバイス配列
	    /// </summary>
	    /// <param name="value">BinaryHex文字列データ（16進数を文字列で指定）</param>
	    /// <remarks></remarks>
	    public int[] ConvertBinaryAsWord(string value, int size, int decimalLength = 0)
	    {
            int dataSize = ((size == 0) ? (int)((value.Length + 3) / 4) : (int)(((size * 4) + 3) / 4));
		    int[] data = new int[dataSize];
		    if ((value.Length < 1)) {
			    value = "0";
		    }
		    // 数値からバイナリ項目書込用の配列を生成する
		    long workValue = long.Parse(value, System.Globalization.NumberStyles.HexNumber) * (long)Math.Pow(10, decimalLength);
		    for (int i = 0; i <= (data.Length - 1); i += 1) {
			    data[i] = (int)(workValue % 65536);
			    workValue >>= 16;
		    }
		    return data;
	    }

	    /// <summary>
	    /// ワードデバイスBINARY書き込み処理
	    /// </summary>
	    /// <param name="address">書込先頭アドレス</param>
	    /// <param name="value">書込データ（16進数を文字列で指定）</param>
	    /// <remarks></remarks>
	    public void WriteWordAsBinary(string address, string value, int size, int decimalLength = 0)
	    {
		    // データ書き込み
		    int[] data = this.ConvertBinaryAsWord(value, size, decimalLength);
		    if (!this.WriteWord(address, data)) {
			    throw new IOException(string.Format("PLCワードデバイスへのBINARY書き込みに失敗しました。Address:[{0}]/Size:[{1}]", address, data.Length));
		    }
	    }

	    /// <summary>
	    ///BCD文字列→ワードデバイス配列
	    /// </summary>
	    /// <param name="value">BCD文字列データ</param>
	    /// <remarks></remarks>
	    public int[] ConvertBCDAsWord(string value, int size)
	    {
            int dataSize = ((size == 0) ? (int)((value.Length + 3) / 4) : (int)((size + 3) / 4));
		    int[] data = new int[dataSize];
		    value = value.Trim().PadLeft((dataSize) * 4).Replace(" ", "0");
		    // 文字列（数値）からBCD項目書込用配列を生成する
            for (int i = 0; i < dataSize; i++)
            {
			    int workValue = 0;
			    int shiftValue = 4096;
			    for (int j = 0; j <= 3; j++) {
                    string targetValue = value.Substring((i * 4) + j, 1);
				    workValue += ((targetValue.Length > 0) ? int.Parse(targetValue) : 0) * shiftValue;
				    shiftValue /= 16;
			    }
			    data[i] = workValue;
		    }
		    return data;
	    }

	    /// <summary>
	    /// ワードデバイスBCD書き込み処理
	    /// </summary>
	    /// <param name="address">書込先頭アドレス</param>
	    /// <param name="value">書込データ</param>
	    /// <remarks></remarks>
	    public void WriteWordAsBcd(string address, string value, int size)
	    {
		    // データ書き込み
		    int[] data = this.ConvertBCDAsWord(value, size);
		    if (!this.WriteWord(address, data)) {
			    throw new IOException(string.Format("PLCワードデバイスへのBCD書き込みに失敗しました。Address:[{0}]/Size:[{1}]", address, data.Length));
		    }
	    }
    }
}
