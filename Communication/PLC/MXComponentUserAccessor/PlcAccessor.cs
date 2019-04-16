namespace BAMSS.MXComponent
{
    /// <summary>
    /// PLCアクセスの基底クラス（Root）
    /// </summary>
    public abstract class PlcAccessor
    {
        protected MXComponentCoreAccessor _plcAccessor;
        protected PlcConnectionInfo _plcConnectionInfo;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionID"></param>
        /// <param name="plcConfLoader"></param>
        public PlcAccessor(string connectionID, PlcConfigLoader plcConfLoader)
        {
            this._plcConnectionInfo = plcConfLoader.LoadPLCConnectionInfo(connectionID);
            this._plcAccessor = MXComponentCoreAccessor.GetInstance(this._plcConnectionInfo.ActLogicalNo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plcConnectionInfo"></param>
        public PlcAccessor(PlcConnectionInfo plcConnectionInfo)
        {
            this._plcConnectionInfo = plcConnectionInfo;
            this._plcAccessor = MXComponentCoreAccessor.GetInstance(this._plcConnectionInfo.ActLogicalNo);
        }
    }
}
