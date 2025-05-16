using B1Core.Business;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core
{
    /// <summary>
    /// sapb1 işlemleri bu değişken üzerinden yapulcuk, sarumanın doğuşu buradan başlayacak
    /// </summary>
    public static class Main
    {
        public static SAPbobsCOM.Company oCompany { get; set; }

        public static bool SetupControl(string tableName, int version)
        {
            try
            {
                var ver = Data.ExecuteSql($"select \"U_Value\" from \"@{tableName}\" where \"Name\"='Version'").Data.Fields.Item("U_Value").Value.ToString();
                if (ver == "")
                {
                    UserTable oUserTable = Main.oCompany.UserTables.Item(tableName);
                    oUserTable.Name = "Version";
                    oUserTable.UserFields.Fields.Item("U_Value").Value = (version - 1).ToString();
                    oUserTable.Add();

                    ver = (version - 1).ToString();
                }

                if (int.Parse(ver) >= version)
                {
                    return true;
                }

                var versData = Data.ExecuteSql($"update \"@{tableName}\" set \"U_Value\"='{version}' where \"Name\"='Version'");

                return false;
            }
            catch 
            {
                return false;
            }
        }

        public static bool IsHana { get { return oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB ? true : false; } }

        public static string GetQueryDummy()
        {
            if (IsHana)
                return " FROM DUMMY;";
            else
                return "";
        }
    }
}
