
using B1Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Initial
{
    public static class ProcCreate
    {
        
        public static bool CreateProcedure(string sql, string procName)
        {
            try
            {
                Data.ExecuteSql("DROP PROCEDURE \"" + procName + "\"");
            }
            catch { }

            if (Main.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                Data.ExecuteSql(sql);
                return true;
            }
            else
            {
                return false;
            }
        }

       
        public static bool CreateView(string sql, string viewName)
        {
            try
            {
                Data.ExecuteSql("DROP VIEW \"" + viewName + "\"");
            }
            catch { }

            //if (Main.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            //{
                Data.ExecuteSql(sql);
                return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public static bool CreateTrigger(string sql, string triggerName)
        {
            try
            {
                Data.ExecuteSql("DROP TRIGGER \"" + triggerName + "\"");
            }
            catch { }

            if (Main.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                Data.ExecuteSql(sql);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CreateFunction(string sql, string function)
        {
            try
            {
                Data.ExecuteSql("DROP FUNCTION \"" + function + "\"");
            }
            catch { }

            if (Main.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                Data.ExecuteSql(sql);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}