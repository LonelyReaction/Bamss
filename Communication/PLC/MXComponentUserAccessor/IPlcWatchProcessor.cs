namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLC通信処理呼出制御側処理用のインターフェイス
    /// </summary>
    /// <remarks></remarks>
    public interface IPlcWatchProcessor
    {
        /// <summary>
        /// 処理名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 定周期処理
        /// </summary>
        void WatchEvent();
    }
}
