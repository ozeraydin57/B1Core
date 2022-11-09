//using B1Core.Business.UI;
//using B1Core.Entity;
//using B1CoreEntities.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace B1Core.Business
//{
//    public static class DataOBj<T>
//    {
//        public static Response<EDocDocument> ReadDocumentData(string table, Dictionary<string, string> whereColumnValue)
//        {
//            var resp = new Response<EDocDocument>();

//            string sql = string.Format("SELECT TOP 1 * FROM \"{0}\" T0 WHERE 1=1 ", table);

//            foreach (var item in whereColumnValue)
//            {
//                sql += string.Format(" AND T0.\"{0}\" = '{1}'", item.Key, item.Value);
//            }

//            var ret = Data.ExecuteSql(sql);
//            if (ret.Success)
//            {
//                var doc = new EDocDocument();

//                doc.DocScenarioType = ret.Data.Fields.Item("DocScenarioType").Value.ToString();
//                doc.DocInvoiceType = ret.Data.Fields.Item("DocInvoiceType").Value.ToString();
//                doc.CurrencyCode = ret.Data.Fields.Item("CurrencyCode").Value.ToString();



//                resp.Data = doc;
//                resp.RecordCount = ret.RecordCount;
//            }
//            resp.Success = ret.Success;
//            resp.Message = ret.Message;

//            return resp;
//        }



//    }
//}
