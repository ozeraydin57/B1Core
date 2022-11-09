using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace B1Core.Business.UI
{
    public static class Grid
    {
        public static void LinkedColumn(SAPbouiCOM.Grid grid0, string column, string objectType)
        {
            try
            {
                var edit3 = (SAPbouiCOM.EditTextColumn)(grid0.Columns.Item(column));
                edit3.LinkedObjectType = objectType;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Hata N103: " + ex.ToString());
            }
        }

        public static void CheckBoxColumn(SAPbouiCOM.Grid grid, string column)
        {
            try
            {
                grid.Columns.Item(column).Type = BoGridColumnType.gct_CheckBox;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Hata N106: " + ex.ToString());
            }
        }

        public static void Editable(SAPbouiCOM.Grid grid, bool editable)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                grid.Columns.Item(i).Editable = false;
            }
        }

        public static void PictureColumn(SAPbouiCOM.Grid grid, string column)
        {
            grid.Columns.Item(column).Type = BoGridColumnType.gct_Picture;
        }

        /// <summary>
        /// kolon title değiştirir row header türkçe vs.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="column"></param>
        /// <param name="title"></param>
        public static void ColumnCaption(SAPbouiCOM.Grid grid, string column, string title)
        {
            grid.Columns.Item(column).TitleObject.Caption = title;
        }
    }
}
