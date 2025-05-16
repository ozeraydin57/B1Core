using B1Core.Business;
using B1Core.Business.UI;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Initial
{
    public static class ParamCreate
    {
        public static void AddParams(string table, string param, string value = "", string desc = "")
        {
            try
            {
                var data = Data.ExecuteSql($"select * from \"@{table}\" where \"Name\"='{param}' ");
                if (data.RecordCount == 0)
                {
                    UserTable oUserTable = Main.oCompany.UserTables.Item(table);

                    oUserTable.Name = param;
                    oUserTable.UserFields.Fields.Item("U_Value").Value = value;
                    oUserTable.UserFields.Fields.Item("U_Desc").Value = desc;
                    oUserTable.Add();

                }
            }
            catch (Exception ex)
            {
                StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, "Hata : " + ex.ToString());
            }
        }
    }
}