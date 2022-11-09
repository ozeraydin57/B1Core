using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace B1CoreCore.Business.UI
{
    public static class Button
    {
        public static void PressBefore(SAPbouiCOM.Button btn)
        {
            try
            {
                btn.Item.Width = btn.Item.Width + 1;
                btn.Item.Height = btn.Item.Height + 1;
            }
            catch
            {

            }
        }
        public static void PressAfter(SAPbouiCOM.Button btn)
        {
            try
            {
                btn.Item.Width = btn.Item.Width - 1;
                btn.Item.Height = btn.Item.Height - 1;
            }
            catch
            {

            }
        }
    }
}
