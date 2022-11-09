using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreCore.Business
{
    public static class DataHelper
    {
        public static string GetValueString(Recordset data, string column)
        {
            try
            {
                return data.Fields.Item(column).Value.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Hata N101: " + column + " " + ex.ToString());
                return "";
            }
        }
        public static Decimal GetValueDecimal(Recordset data, string column)
        {
            try
            {
                var val = GetValueString(data, column);
                if (string.IsNullOrEmpty(val))
                    return 0;
                return Convert.ToDecimal(val);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Hata N102: " + ex.ToString());
                return 0;
            }
        }
        public static DateTime GetValueDate(Recordset data, string column)
        {
            try
            {
                var val = GetValueString(data, column);

                return Convert.ToDateTime(val);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Hata N104: " + ex.ToString());
                return new DateTime(1955, 1, 1);
            }
        }
        public static DateTime GetValueTime(Recordset data, string column, string columDate)
        {
            try
            {
                var time = GetValueString(data, column).PadLeft(4,'0');
                var val1 = GetValueString(data, columDate);
                var dt = Convert.ToDateTime(val1);
                var date = new DateTime(dt.Year, dt.Month, dt.Day, int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2, 2)),0);

                return date;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Hata N104: " + ex.ToString());
                return new DateTime(1955, 1, 1);
            }
        }
        public static int GetValueInt(Recordset data, string column)
        {
            try
            {
                var val = GetValueString(data, column);
                if (string.IsNullOrEmpty(val))
                    return 0;
                return Convert.ToInt32(val);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Hata N105: " + ex.ToString());
                return 0;
            }
        }
    }
}

