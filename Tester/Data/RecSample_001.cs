namespace BAMSS.Data
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// サンプルテーブルレコードクラス1
    /// </summary>
    public partial class RecSample_001 : RecordBase
    {
        public int LINE_ID { get; set; }
        public string SEIHIN_CODE { get; set; }
        public int GROUP_NO { get; set; }
        public int TIMING_NO { get; set; }
        public int TARGET_VISC { get; set; }
        public int ELAPSED_TIME { get; set; }
        public int CONTINUE_JUDGE_TIME { get; set; }
        public DateTime UPDATE_DATE { get; set; }
    }
}
