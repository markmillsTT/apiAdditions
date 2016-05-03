using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTechnologies.TTAPI;

namespace TimeAndSalesOrganizer
{
    public static class AppProperties
    {
        public static string fileLoc = @"..\..\..\Resources\rawData";
        public static string m_username = "MARK";
        public static string m_password = "12345678";
        public static ProductKey prod1Key = new ProductKey("CME", "FUTURE", "ES");
        public static string contract1Key = "Mar16";
    }
}
