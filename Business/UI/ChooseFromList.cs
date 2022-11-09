using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreCore.Business.UI
{
    public class ChooseFromList
    {

        public static void SelectAfter(SAPbouiCOM.EditText editText, string alias, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.Form oForm = B1CoreCore.Business.UI.Form.GetActiveForm();
                SAPbouiCOM.ISBOChooseFromListEventArg cflEventarg = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
                SAPbouiCOM.DataTable dt = cflEventarg.SelectedObjects;
                if (dt != null)
                {
                    string val = System.Convert.ToString(dt.GetValue(alias, 0));
                    try
                    {
                        editText.String = val;
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                StatusBar.SetMessage(BoStatusBarMessageType.smt_Error, "Seçim sırasında hata: " + ex.Message);
            }
        }

        /// <summary>
        /// edittext chosefromlistbefore da
        /// </summary>
        /// <param name="cflId"></param>
        /// <param name="condt">condisyonlar</param>
        public static void SelectBefore(string cflId, List<EntKeyValue> condt, BoConditionRelationship clouseOrAnd = BoConditionRelationship.cr_AND)
        {
            try
            {
                SAPbouiCOM.ChooseFromList oCFL;
                SAPbouiCOM.Condition oCon = default(SAPbouiCOM.Condition);

                SAPbouiCOM.Conditions oCons = default(SAPbouiCOM.Conditions);
                SAPbouiCOM.Form oForm = Form.GetActiveForm();

                //if (condt != null)
                //{
                string CFLID = cflId;
                oCFL = oForm.ChooseFromLists.Item(CFLID);

                oCFL.SetConditions(null);
                oCons = oCFL.GetConditions();

                int i = 0;
                foreach (var item in condt)
                {
                    if (i > 0)
                        oCon.Relationship = clouseOrAnd;
                    i++;

                    oCon = oCons.Add();
                    oCon.Alias = item.Key;
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = item.Value;
                }
                oCFL.SetConditions(oCons);

                //}
            }
            catch
            {

            }
        }

    }

    public class EntKeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
