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
    }

    public static bool SetupControl(string tableName, int version) //version for setup object
    {
            try
            {
                var ver = Data.ExecuteSql($"select U_Value from [@{tableName}] where Name='Version'").Data.Fields.Item("U_Value").Value.ToString();
                if (ver == "")
                {
                    Data.ExecuteSql($"insert into [@{tableName}] (Name, U_Value) values('Version', {version - 1}) ");
                    ver = (version - 1).ToString();
                }

                if (int.Parse(ver) >= version)
                {
                    return true;
                }

                var versData = Data.ExecuteSql($"update [@{tableName}] set U_Value='{int.Parse(ver) + 1}' where Name='Version'");

                return false;
            }
            catch
            {
                return false;
            }
 }
}
