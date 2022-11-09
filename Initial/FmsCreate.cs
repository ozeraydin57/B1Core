using B1CoreCore.Business;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreCore.Initial
{
    public static class FmsCreate
    {
        //User Query ve Formatted Searchleri oluşturan fonksiyondur.
        public static void AddFms(string fmsName, string query, string formID, string itemID, string colID)
        {
            int errCode;
            string errMsg;

            SAPbobsCOM.FormattedSearches oFS = (SAPbobsCOM.FormattedSearches)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches);

            SAPbobsCOM.UserQueries oUserQuery = (SAPbobsCOM.UserQueries)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);

            //User Query var mı kontrolü yapar.
            if (!QueryExist(fmsName))
            {
                Application.SBO_Application.StatusBar.SetText("Fms oluşturuluyor " + fmsName + " ..........Lütfen Bekleyiniz.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                oUserQuery.QueryDescription = fmsName;

                oUserQuery.Query = query;

                oUserQuery.QueryType = SAPbobsCOM.UserQueryTypeEnum.uqtWizard;

                oUserQuery.QueryCategory = -1;

                errCode = oUserQuery.Add();

                Main.oCompany.GetLastError(out errCode, out errMsg);

                if (errCode != 0)
                {
                    Application.SBO_Application.StatusBar.SetText("Fms hata " + fmsName + " ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText("Fms oluşturuldu " + fmsName + " ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
            }

            if (!QueryExist(fmsName) || !formattedSearchExist(formID, itemID, colID))
            {

                string sqlClause = "select \"IntrnalKey\" from \"OUQR\" where \"QName\" = '" + fmsName + "'";

                SAPbobsCOM.Recordset oRecSet = Data.ExecuteSql(sqlClause).Data;

                int intrnalKey = Convert.ToInt32(oRecSet.Fields.Item(0).Value);

                oFS.FormID = formID;

                oFS.ItemID = itemID;

                oFS.ColumnID = colID;

                oFS.Action = SAPbobsCOM.BoFormattedSearchActionEnum.bofsaQuery;

                oFS.QueryID = intrnalKey;

                oFS.ByField = SAPbobsCOM.BoYesNoEnum.tYES;


                errCode = oFS.Add();

                Main.oCompany.GetLastError(out errCode, out errMsg);


            }

            oFS = null;
            oUserQuery = null;

            TableCreate.GCClear();
        }


        //Verilen isimli user query var mı kontrol eder.
        private static bool QueryExist(string queryName)
        {
            try
            {
                SAPbobsCOM.Recordset recordSet = Data.ExecuteSql("SELECT  COUNT(1) as \"COUNT\" from \"OUQR\" WHERE  \"QName\" = '" + queryName + "'").Data;

                recordSet.MoveFirst();

                if (Convert.ToInt32(recordSet.Fields.Item("COUNT").Value.ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                Logger.WriteErrorLog("Fms kurulum hata" + exc.ToString());
                return true;
            }
        }


        //Verilen isimli formatted search var mı kontrol eder.
        private static bool formattedSearchExist(string formID, string itemID, string colID)
        {
            try
            {
                SAPbobsCOM.Recordset recordSet = Data.ExecuteSql("SELECT  COUNT(1) as \"COUNT\" from \"CSHS\" WHERE  \"FormID\" = '" + formID + "' and \"ItemID\" = '" + itemID + "' and \"ColID\" = '" + colID + "'").Data;

                recordSet.MoveFirst();

                if (Convert.ToInt32(recordSet.Fields.Item("COUNT").Value.ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                Logger.WriteErrorLog("Formatted search kontrolünde hata" + exc.ToString());
                // hata durumunda drop etsin diye
                return true;
            }
        }
    }
}

