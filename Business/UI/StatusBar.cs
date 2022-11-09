using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Business.UI
{
    public static class StatusBar
    {
        public static void SetMessage(SAPbouiCOM.BoStatusBarMessageType messageType, string message, SAPbouiCOM.BoMessageTime messageTime = BoMessageTime.bmt_Medium)
        {
            SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(message, messageTime, messageType);
            Logger.WriteErrorLog(messageType.ToString() + " " + message);
        }
    }
}
