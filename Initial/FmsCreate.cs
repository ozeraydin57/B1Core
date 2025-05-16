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

            var dt = Data.ExecuteSql($"select \"IndexID\" from \"CSHS\" where \"FormID\"='{formId}' and \"ItemID\"='{ItemId}' and \"ColID\"='{colId}' ");
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

                //if (colIdRelation2 != "")
                //{
                //    oFS.FieldIDs.Add();
                //    oFS.FieldIDs.FieldID = colIdRelation2;
                //}
            }

            if (find)
                oFS.Update();
            else
                oFS.Add();

            var rrr = Main.oCompany.GetLastErrorDescription();

            TableCreate.GCClear();
        }

        public static void FmsRemove(string categoryName)
        {
            Data.ExecuteSql($"delete from \"OQCN\" where \"CatName\" in ('{categoryName}')");
            Data.ExecuteSql("delete from \"OUQR\" where \"QCategory\" in (select distinct \"QCategory\" from \"OUQR\" left join \"OQCN\" on \"QCategory\" = \"CategoryId\" where \"CategoryId\" is null)");
            Data.ExecuteSql("delete from \"CSHS\" where \"QueryId\" not in (select \"IntrnalKey\" from \"OUQR\" )");
        }


        public static int GetCategoryId(string categoryName)
        {
            FmsRemove(categoryName);

            QueryCategories categories = (QueryCategories)Main.oCompany.GetBusinessObject(BoObjectTypes.oQueryCategories);
            categories.Name = categoryName;
            categories.Permissions = "ALL_GROUPS_ALLOWED";
            var RetVal = categories.Add();

            int categoryId = int.Parse(Data.ExecuteSql($"SELECT * FROM \"OQCN\" WHERE \"CatName\"='{categoryName}'").Data.Fields.Item("CategoryId").Value.ToString());

            return categoryId;
        }

        public static string GetTableFormId(string tableName)
        {
            return Data.ExecuteSql($"select '11'+right('00000'+CAST(\"TblNum\" as varchar(50)),3 ) \"FormId\" from \"OUTB\"  where \"TableName\"='{tableName}' ").Data.Fields.Item("FormId").Value.ToString();
        }
    }
}