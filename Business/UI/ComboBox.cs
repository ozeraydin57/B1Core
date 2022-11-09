using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Business.UI
{
    public static class ComboBox
    {

        /// <summary>
        /// sorguya göre combo yu doldurur, önce varolan dğerleri uçurtur
        /// </summary>
        /// <param name="cmb">combo</param>
        /// <param name="query">query 1. alan value, 2. alan tanım olacak</param>
        public static void Fill(SAPbouiCOM.ComboBox cmb, string query)
        {
            try
            {
                Clear(cmb);

                try
                {
                    cmb.ValidValues.Add("", "");
                }
                catch { }

                var rec = Data.ExecuteSql(query);

                if (rec.Success && rec.Data.RecordCount > 0)
                {
                    for (int i = 0; i < rec.Data.RecordCount; i++)
                    {
                        try
                        {
                            cmb.ValidValues.Add(rec.Data.Fields.Item(0).Value.ToString(), rec.Data.Fields.Item(1).Value.ToString());
                        }
                        catch { }

                        rec.Data.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("FillCombobox cmb:" + cmb.Item.UniqueID + " query:" + query + "  \n" + ex.ToString());
            }
        }

        public static void Clear(SAPbouiCOM.ComboBox cmb)
        {
            try
            {
                if (cmb.ValidValues.Count > 0)
                {
                    int count = cmb.ValidValues.Count;
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            cmb.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                        }
                        catch { }
                    }
                    cmb.ValidValues.Add("", "");
                    cmb.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ClearCombobox cmb:" + cmb.Item.UniqueID + " \n" + ex.ToString());
            }
        }
    }
}
