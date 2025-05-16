using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace B1Core.Business.UI
{
    public static class Form
    {
        public static SAPbouiCOM.Form GetActiveForm()
        {
            var form = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;

            if (form.Type < 0)
            {
                string type = (form.Type * -1).ToString();
                form = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(type, form.TypeCount);
            }

            return form;
        }

        public static void SetCenter()
        {
            var form = GetActiveForm();

            form.Left = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Width - form.Width) / 3;
            form.Top = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Height - form.Height) / 5;
        }

        public static void AddButton(SAPbouiCOM.Form oForm, string uniqId, int pane, string caption, int top, int left, int width, int height)
        {
            SAPbouiCOM.Item oItem = null;
            try
            {
                oItem = oForm.Items.Item(uniqId);
            }
            catch
            {
            }
            SAPbouiCOM.Button oButton = null;

            if (oItem == null)
            {
                oItem = oForm.Items.Add(uniqId, SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            }
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Item.Visible = false;

            oButton.Caption = caption;
            oButton.Item.Top = top;
            oButton.Item.Left = left;
            oButton.Item.Width = width;
            oButton.Item.Height = height;
            oButton.Item.ToPane = pane;
            oButton.Item.FromPane = pane;

            oButton.Item.Visible = true;
        }

        public static void SetCenter(SAPbouiCOM.ItemEvent pVal)
        {
            var form = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount);

            form.Left = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Width - form.Width) / 3;
            form.Top = (SAPbouiCOM.Framework.Application.SBO_Application.Desktop.Height - form.Height) / 5;
        }

        public static void Refresh()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.ActivateMenuItem("1304");
        }
        public static void AddMode()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.ActivateMenuItem("1282");
        }

        public static void AddCheckBox(SAPbouiCOM.Form oForm, string table, string field, string uniqId, int pane, int top, int left, int width, int height)
        {
            SAPbouiCOM.Item oItem = null;
            try
            {
                oItem = oForm.Items.Item(uniqId);
            }
            catch
            {
            }

            SAPbouiCOM.CheckBox oEditText = null;

            if (oItem == null)
            {
                oItem = oForm.Items.Add(uniqId, SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            }

            oEditText = ((SAPbouiCOM.CheckBox)(oItem.Specific));
            if (table != "")
                oEditText.DataBind.SetBound(true, table, field);

            // oEditText.Item.Visible = false;

            oEditText.Item.Top = top;
            oEditText.Item.Left = left;
            oEditText.Item.Width = width;
            oEditText.Item.Height = height;
            oEditText.Item.ToPane = pane;
            oEditText.Item.FromPane = pane;

            //oEditText.Value = "";

            oEditText.Item.DisplayDesc = true;
            oEditText.Item.Visible = true;
            //oEditText.Item.AffectsFormMode = false;
        }

        public static void AddFolder(SAPbouiCOM.Form oForm, string folderId, string caption, string folderBaseId)
        {
            SAPbouiCOM.Item oItem = null;
            try
            {
                oItem = oForm.Items.Item(folderId);
            }
            catch
            {
            }
            SAPbouiCOM.Folder oFolder = null;
            if (oItem == null)
            {
                oItem = oForm.Items.Add(folderId, SAPbouiCOM.BoFormItemTypes.it_FOLDER);
            }
            var folderBase = (SAPbouiCOM.Folder)(oForm.Items.Item(folderBaseId).Specific);

            oFolder = ((SAPbouiCOM.Folder)(oItem.Specific));
            oFolder.Caption = caption;
            oFolder.Pane = 1;
            oFolder.AutoPaneSelection = true;
            oFolder.Item.Left = folderBase.Item.Left + folderBase.Item.Width;
            oFolder.Item.Width = folderBase.Item.Width;
            oFolder.Item.Top = folderBase.Item.Top;
            oFolder.Item.Height = folderBase.Item.Height;
            oFolder.GroupWith(folderBaseId);
            oFolder.Item.AffectsFormMode = false;

            oFolder.Item.Visible = true;
            oForm.PaneLevel = 1;
            folderBase.Item.Click();
        }

        public static SAPbouiCOM.EditText AddEditText(SAPbouiCOM.Form oForm, string table, string field, string uniqId, int pane, int top, int left, int width, int height, bool visible = true)
        {
            SAPbouiCOM.Item oItem = null;
            try
            {
                oItem = oForm.Items.Item(uniqId);
            }
            catch
            {
            }

            SAPbouiCOM.EditText oEditText = null;

            if (oItem == null)
            {
                oItem = oForm.Items.Add(uniqId, SAPbouiCOM.BoFormItemTypes.it_EDIT);
            }

            oEditText = ((SAPbouiCOM.EditText)(oItem.Specific));
            if (table != "")
                oEditText.DataBind.SetBound(true, table, field);

            // oEditText.Item.Visible = false;

            oEditText.Item.Top = top;
            oEditText.Item.Left = left;
            oEditText.Item.Width = width;
            oEditText.Item.Height = height;
            oEditText.Item.ToPane = pane;
            oEditText.Item.FromPane = pane;

            //oEditText.Value = "";
            oEditText.Item.Visible = visible;

            //oEditText.Item.AffectsFormMode = false;

            return oEditText;
        }

        public static SAPbouiCOM.ComboBox AddComboBox(SAPbouiCOM.Form oForm, string table, string field, string uniqId, int pane, int top, int left, int width, int height)
        {
            SAPbouiCOM.Item oItem = null;
            try
            {
                oItem = oForm.Items.Item(uniqId);
            }
            catch
            {
            }

            SAPbouiCOM.ComboBox oEditText = null;

            if (oItem == null)
            {
                oItem = oForm.Items.Add(uniqId, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
            }

            oEditText = ((SAPbouiCOM.ComboBox)(oItem.Specific));
            if (table != "")
                oEditText.DataBind.SetBound(true, table, field);

            // oEditText.Item.Visible = false;

            oEditText.Item.Top = top;
            oEditText.Item.Left = left;
            oEditText.Item.Width = width;
            oEditText.Item.Height = height;
            oEditText.Item.ToPane = pane;
            oEditText.Item.FromPane = pane;

            //oEditText.Value = "";

            oEditText.Item.DisplayDesc = true;
            oEditText.Item.Visible = true;
            //oEditText.Item.AffectsFormMode = false;
            return oEditText;
        }


        public static void AddStatic(SAPbouiCOM.Form oForm, string caption, string uniqId, int pane, int top, int left, int width, int height, bool visible = true)
        {
            SAPbouiCOM.Item oItem = null;
            try
            {
                oItem = oForm.Items.Item(uniqId);
            }
            catch
            {
            }

            SAPbouiCOM.StaticText oEditText = null;

            if (oItem == null)
            {
                oItem = oForm.Items.Add(uniqId, SAPbouiCOM.BoFormItemTypes.it_STATIC);
            }

            oEditText = ((SAPbouiCOM.StaticText)(oItem.Specific));

            // oEditText.Item.Visible = false;

            oEditText.Item.Top = top;
            oEditText.Item.Left = left;
            oEditText.Item.Width = width;
            oEditText.Item.Height = height;
            oEditText.Item.ToPane = pane;
            oEditText.Item.FromPane = pane;
            oEditText.Caption = caption;

            //oEditText.Value = "";
            oEditText.Item.Visible = visible;

            //oEditText.Item.AffectsFormMode = false;
        }
    }
}
