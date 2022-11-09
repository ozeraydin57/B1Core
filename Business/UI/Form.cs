using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreCore.Business.UI
{
    public static class Form
    {
        public static SAPbouiCOM.Form GetActiveForm()
        {
            return SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
        }

        public static void SetCenter()
        {
            var form = GetActiveForm();

            form.Left = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Width - form.Width) / 3;
            form.Top = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Height - form.Height) / 5;
        }

        //public static SAPbouiCOM.Form GetActiveForm(SAPbouiCOM.SBOItemEventArg pVal)
        //{
        //    return SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(pVal.FormUID, pVal.FormTypeCount);

        //}

        public static void SetCenter(SAPbouiCOM.ItemEvent pVal)
        {
            var form = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount);

            form.Left = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Width - form.Width) / 3;
            form.Top = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Height - form.Height) / 5;
        }

        
    }
}
