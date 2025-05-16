using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Business.UI
{
    public static class Menu
    {
        public static void CreateMenuItem(string menuId, string menuTitle, string formName, int order)
        {
            try
            {
                var oMenuItem = SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item(menuId);
                //oMenuItem = oMenuItem.SubMenus.Item("Başlangıç");
                var oMenus = oMenuItem.SubMenus;
                var oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(SAPbouiCOM.Framework.Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
                CreateMenuItem(oMenus, oCreationPackage, menuTitle, formName, order);
            }
            catch
            {

            }
        }

        public static void CreateMenuItem(SAPbouiCOM.Menus oMenus, SAPbouiCOM.MenuCreationParams oCreationPackage, string text, string uniqueID, int order)
        {
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            oCreationPackage.UniqueID = uniqueID;
            oCreationPackage.String = text;
            oCreationPackage.Position = order;
            oMenus.AddEx(oCreationPackage);
        }

        public static void CreateMenuItemMainMenu(string id, string title, string logo = "", string menuId = "43520")
        {
            SAPbouiCOM.Menus oMenus = Application.SBO_Application.Menus;
            try
            {
                oMenus.RemoveEx(id);
            }
            catch
            {

            }
            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            SAPbouiCOM.MenuItem oMenuItem = Application.SBO_Application.Menus.Item(menuId);

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = id;
            oCreationPackage.String = title;
            oCreationPackage.Position = 20;

            if (logo != "")
                oCreationPackage.Image = AppDomain.CurrentDomain.BaseDirectory + "Statik\\" + logo;

            oMenus = oMenuItem.SubMenus;
            oMenus.AddEx(oCreationPackage);
        }

        public static void NavigateMenu(string table)
        {
            string sqlMenu = " select \"MenuUId\",\"TableName\" from ( " +
                             " select(RANK() OVER(ORDER BY \"TableName\")) + 51200 AS \"MenuUId\", \"TableName\" from \"OUTB\"  where \"UsedInObj\" is null and \"ObjectType\" in (5,0) " +
                             $" ) T0 where T0.\"TableName\" = '{table}'";
            try
            {
                string menuItem = Data.ExecuteSql(sqlMenu).Data.Fields.Item("MenuUid").Value.ToString();
                SAPbouiCOM.Framework.Application.SBO_Application.ActivateMenuItem(menuItem);
            }
            catch (Exception ex)
            {
                B1Core.Business.UI.StatusBar.SetMessage(SAPbouiCOM.BoStatusBarMessageType.smt_Error, ex.ToString());
            }
        }
    }
}
