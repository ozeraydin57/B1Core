using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreCore.Business.UI
{
    public static class DbDataSource
    {
        /// <summary>
        /// formdaki datasource doldurmak için kullanılıyor
        /// </summary>
        /// <param name="oDBDataSource"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public static void Fill(SAPbouiCOM.DBDataSource oDBDataSource, string field, string value)
        {
            try
            {
                var oConditions = new SAPbouiCOM.Conditions();
                var oCondition = oConditions.Add();

                oCondition.BracketOpenNum = 1;

                oCondition.ComparedAlias = field;
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;

                oCondition.CondVal = value;
                oCondition.BracketCloseNum = 1;

                oDBDataSource.Query(oConditions);
            }
            catch (Exception ex)
            {
                StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, "Hata : " + ex.ToString());
            }
        }
    }
}
