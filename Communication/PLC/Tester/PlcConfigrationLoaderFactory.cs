using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAMSS.MXComponent;

namespace Tester
{
    public static class PlcConfigrationLoaderFactory
    {
        private static string dbConnectionString = "Data Source=BAMSS-W122SS12;Initial Catalog=TFC_DB;User ID=N1819_sqluser;Password=ChiN1819_sqluser;Network Library=dbmssocn;Connection Timeout=15;";
        public static PlcConfigLoaderSQLServer LoadPlcConfigFromSQLServer()
        {
            return new PlcConfigLoaderSQLServer(PlcConfigrationLoaderFactory.dbConnectionString);
        }
    }
}
