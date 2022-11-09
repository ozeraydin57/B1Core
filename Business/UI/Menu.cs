using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreCore.Business.UI
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
        public static void CreateMenuItemWithSubMenu(SAPbouiCOM.Menus oMenus, SAPbouiCOM.MenuItem oMenuItem, SAPbouiCOM.MenuCreationParams oCreationPackage, string text)
        {
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = text;
            oCreationPackage.String = text;
            oMenus = oMenuItem.SubMenus;
            oMenus.AddEx(oCreationPackage);
        }

    }
}
