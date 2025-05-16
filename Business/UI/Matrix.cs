using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace B1Core.Business.UI
{
    public static class Matrix
    {
        public static void AddRow(SAPbouiCOM.Matrix matrix0, int rows = 1, bool lineIdAdd = true)
        {
            try
            {
                matrix0.AddRow(rows);
                var count = matrix0.VisualRowCount;
                matrix0.ClearRowData(count);

                if (count == 1)
                    return;

                if (!lineIdAdd)
                    return;

                var lineId = (SAPbouiCOM.EditText)matrix0.Columns.Item("LineId").Cells.Item(count).Specific;
                string lineOnc = ((SAPbouiCOM.EditText)matrix0.Columns.Item("LineId").Cells.Item(count - 1).Specific).String;

                if (lineOnc == "") lineOnc = "1";

                var lineIdOnceki = int.Parse(lineOnc);

                lineId.String = (lineIdOnceki + 1).ToString();
            }
            catch (Exception ex)
            {
                B1Core.Business.UI.StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, "Hata: " + ex.Message);
            }
        }

        //public static void AddRowLineTable(SAPbouiCOM.Matrix matrix0, string lineTable, int docEntry, int rows = 1)
        //{
        //    try
        //    {
        //        matrix0.AddRow(rows);
        //        var count = matrix0.VisualRowCount;
        //        matrix0.ClearRowData(count);

        //        if (count == 1)
        //            return;

        //        var lineId = (SAPbouiCOM.EditText)matrix0.Columns.Item("LineId").Cells.Item(count).Specific;

        //        lineId.String = Data.ExecuteSql($"select Max(\"LineId\")+1 from \"@{lineTable}\" where \"DocEntry\"={docEntry} ").Data.Fields.Item("LineId").Value.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        B1Core.Business.UI.StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, "Hata: " + ex.Message);
        //    }
        //}

        public static void RemoveSelectedRow(SAPbouiCOM.Matrix matrix0)
        {
            try
            {
                int selectedRow = matrix0.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                if (selectedRow > 0)
                {
                    matrix0.DeleteRow(selectedRow);
                    var oForm = B1Core.Business.UI.Form.GetActiveForm();
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                }
            }
            catch (Exception ex)
            {
                B1Core.Business.UI.StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, "Hata: " + ex.Message);
            }
        }

        public static void SubTotal(SAPbouiCOM.Matrix grid, string column)
        {
            var col = (Column)grid.Columns.Item(column);
            col.ColumnSetting.SumType = BoColumnSumType.bst_Auto;
        }

        public static void ColumnEditable(SAPbouiCOM.Matrix matrix, string column, bool editable)
        {
            try
            {
                var item = matrix.Columns.Item(column);
                item.Editable = editable;
            }
            catch (Exception ex)
            {
                StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, "Hata matrix row editable: " + ex.Message);
            }
        }
    }
}