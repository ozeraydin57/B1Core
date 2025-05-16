using B1Core.Business.UI;
using B1Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using System.Data.SqlClient;

namespace B1Core.Business
{
    /// <summary>
    /// recordset ile hızlıca işlem yapmak ve tek noktadan sorgulamaları yönetmek için yapılmıştır.
    /// </summary>
    public static class Data
    {
        public static Response<object> ReadSingleData(string table, string column, string columnWhere = "", string valueWhere = "")
        {
            var dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(columnWhere))
                dic.Add(columnWhere, valueWhere);
            var resp = ReadSingleData(table, column, dic);
            return resp;
        }

        public static Response<object> ReadSingleData(string table, string column, Dictionary<string, string> whereColumnValue)
        {
            var resp = new Response<object>();

            var ret = ReadSingleData(table, whereColumnValue);
            if (ret.Success)
            {
                resp.Data = ret.Data.Fields.Item(column).Value;
                resp.RecordCount = ret.RecordCount;
            }
            else
                resp.Data = "";
            resp.Success = ret.Success;
            resp.Message = ret.Message;

            return resp;
        }

        public static Response<SAPbobsCOM.Recordset> ReadSingleData(string table, Dictionary<string, string> whereColumnValue)
        {
            return ReadListData(table, whereColumnValue, 1);
        }

        public static Response<SAPbobsCOM.Recordset> ReadListData(string table, Dictionary<string, string> whereColumnValue, int selectTop = 0)
        {
            Response<SAPbobsCOM.Recordset> resp = new Response<SAPbobsCOM.Recordset>();
            string top = "";
            if (selectTop > 0) top = " TOP " + selectTop;

            string sql = string.Format("SELECT {1} * FROM \"{0}\" T0 WHERE 1=1 ", table, top);

            foreach (var item in whereColumnValue)
            {
                sql += string.Format(" AND T0.\"{0}\" = N'{1}'", item.Key, item.Value);
            }

            var ret = ExecuteSql(sql);
            if (ret.Success)
            {
                resp.Data = ret.Data;
                resp.RecordCount = ret.RecordCount;
            }
            resp.Success = ret.Success;
            resp.Message = ret.Message;

            return resp;
        }




        /// <summary>
        /// satır silmek için
        /// </summary>
        /// <param name="table">silinen tablo</param>
        /// <param name="columnWhere">koşul için kolon adı</param>
        /// <param name="valueWhere">kolon şartı</param>
        /// <returns></returns>
        public static Response<bool> DeleteData(string table, string columnWhere, string valueWhere)
        {
            if (valueWhere.Length < 1 || columnWhere.Length < 1)
                return new Response<bool>() { Message = "Koşulsuz silinemez", Success = false };

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(columnWhere, valueWhere);

            return DeleteData(table, dic);
        }

        /// <summary>
        /// satır silerrr.
        /// </summary>
        /// <param name="table">tabloadı</param>
        /// <param name="whereColumValue">koşulları kolon ve değeri şeklinde uzatılabilir</param>
        /// <returns></returns>
        public static Response<bool> DeleteData(string table, Dictionary<string, string> whereColumnValue)
        {
            var resp = new Response<bool>();
            if (whereColumnValue.Count < 1)
                return new Response<bool>() { Message = "Koşulsuz silinemez", Success = false };

            string sql = string.Format("DELETE T0 FROM \"{0}\" T0 WHERE 1=1 ", table);

            foreach (var item in whereColumnValue)
            {
                sql += string.Format(" AND T0.\"{0}\" = '{1}'", item.Key, item.Value);
            }

            var rsl = ExecuteSql(sql);

            if (!rsl.Success)
                resp.Message = rsl.Message;
            resp.Success = rsl.Success;

            return resp;
        }


        ///// <summary>
        ///// satır güncelleyeklsfsdfsdf sdfsdf s.
        ///// </summary>
        ///// <param name="table">tabloadı</param>
        ///// <param name="updateColumn">update edilecek kolon ve değerleri</param>
        ///// <param name="whereColumValue">koşulları kolon ve değeri şeklinde uzatılabilir</param>
        ///// <returns></returns>
        //public static Response<SAPbobsCOM.Recordset> UpdateData(string table, Dictionary<string, string> updateColumn, Dictionary<string, string> whereColumnValue)
        //{
        //    Response<SAPbobsCOM.Recordset> resp = new Response<SAPbobsCOM.Recordset>();
        //    if (whereColumnValue.Count < 1)
        //        return new Response<SAPbobsCOM.Recordset>() { Message = "Koşulsuz güncellemeyin", Success = false };

        //    string uptCols = "";
        //    int i = 0;
        //    foreach (var item in updateColumn)
        //    {
        //        if (i > 0)
        //            uptCols += ",";
        //        i++;

        //        uptCols += string.Format(" T0.\"{0}\"='{1}' ", item.Key, item.Value);
        //    }

        //    string sql = string.Format(" UPDATE \"{0}\" T0 SET {1} WHERE 1=1 ", table, uptCols);

        //    foreach (var item in whereColumnValue)
        //    {
        //        sql += string.Format(" AND T0.\"{0}\" = '{1}' ", item.Key, item.Value);
        //    }

        //    return ExecuteSql(sql);
        //}


        /// <summary>
        /// sql çalıştırır ne olduğuna bakmaz, ayrımcılık yapmaz
        /// </summary>
        /// <param name="sql">ver queryi</param>
        /// <returns></returns>
        public static Response<SAPbobsCOM.Recordset> ExecuteSql(string sql, SqlObjectType sqlType = SqlObjectType.Sql)
        {
            Response<SAPbobsCOM.Recordset> res = new Response<SAPbobsCOM.Recordset>();
            try
            {
                SAPbobsCOM.Recordset oRecSetLines = (SAPbobsCOM.Recordset)Main.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                if (sqlType == SqlObjectType.Procedure)
                    sql = ExecSqlProcedure(sql);

                sql = HanaSqlConvert(sql);

                oRecSetLines.DoQuery(sql);
                res.Data = oRecSetLines;
                res.RecordCount = oRecSetLines.RecordCount;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.ToString();
                StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, "Hata : Data ExecuteSql " + sql + "\n" + ex.ToString());
            }
            return res;
        }

        private static string ExecSqlProcedure(string sqlQuery)
        {
            if (Main.oCompany.DbServerType != SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                sqlQuery = sqlQuery.Replace("CALL", "EXEC").Replace("(", " ").Replace(")", "");
            }
            return sqlQuery;
        }

        public static string HanaSqlConvert(string sqlQuery)
        {
            if (Main.oCompany.DbServerType != SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                sqlQuery = sqlQuery.Replace("timestamp", "");
            }

            if (Main.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                sqlQuery = sqlQuery.Replace("isnull", "ifnull");
            }

            return sqlQuery;
        }

         public static Response<bool> ExecuteSqlCommand(string sql, SqlConnection conn)
        {
            var resp = new Response<bool>();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.ToString();
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

            return resp;
        }
    }
}
