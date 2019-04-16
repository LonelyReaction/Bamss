namespace BAMSS.Data
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// サンプルテーブルレコードクラス2
    /// </summary>
    public partial class RecSample_002 : RecordBase
    {
        public int PRODUCT_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public DateTime UPDATE_DATE { get; set; }
    }
}
