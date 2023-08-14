using B1Core.Business;
using SAPbobsCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Initial
{
    public static class FmsCreate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="sql"></param>
        /// <param name="queryName"></param>
        /// <param name="fmsAdd"></param>
        /// <param name="formId"></param>
        /// <param name="itemId">fms konulacak item (matrix de olabilir)</param>
        /// <param name="colId">alan ise -1 matrix ise kolon adı</param>
        /// <param name="colIdRelation">otomatik trigger ettirecek alan</param>
        public static void QueryAdd(int categoryId, string sql, string queryName, bool fmsAdd = false, string formId = "", string itemId = "", string colId = "", string colIdRelation = "")
        {
            UserQueries query = (UserQueries)Main.oCompany.GetBusinessObject(BoObjectTypes.oUserQueries);
            query.Query = sql;
            query.QueryCategory = categoryId;
            query.QueryDescription = queryName;

            var RetVal = query.Add();

            if (fmsAdd)
                FmsAdd(categoryId, queryName, formId, itemId, colId, colIdRelation);
        }

        public static void FmsAdd(int categoryId, string queryName, string formId, string ItemId, string colId, string colIdRelation)
        {
            SAPbobsCOM.Recordset oRecSet = Data.ExecuteSql("select \"IntrnalKey\" from \"OUQR\" where \"QName\" = '" + queryName + "'").Data;
            int intrnalKey = Convert.ToInt32(oRecSet.Fields.Item(0).Value);

            SAPbobsCOM.FormattedSearches oFS = (SAPbobsCOM.FormattedSearches)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches);
            bool find = false;

            var dt = Data.ExecuteSql($"select IndexID from CSHS where FormID='{formId}' and ItemID='{ItemId}' and ColID='{colId}' ");
            if (dt.RecordCount > 0)
            {
                int index = int.Parse(dt.Data.Fields.Item("IndexID").Value.ToString());
                find = oFS.GetByKey(index);
            }

            oFS.FormID = formId;
            oFS.ItemID = ItemId;
            oFS.ColumnID = colId;
            oFS.Action = SAPbobsCOM.BoFormattedSearchActionEnum.bofsaQuery;
            oFS.QueryID = intrnalKey;
            oFS.ByField = SAPbobsCOM.BoYesNoEnum.tYES;
            if (colIdRelation != "")
            {
                oFS.ByFieldEx = FormattedSearchByFieldEnum.fsbfWhenExitingAlteredColumn;
                oFS.Refresh = BoYesNoEnum.tYES;
                oFS.ForceRefresh = BoYesNoEnum.tYES;
                oFS.FieldID = colIdRelation;
            }

            if (find)
                oFS.Update();
            else
                oFS.Add();

            var rrr = Main.oCompany.GetLastErrorDescription();
        }

      
    }
}
