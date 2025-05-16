using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using System.Xml.Linq;

namespace B1Core.Helper
{
    public static class FormHelper
    {
        public static string GetAddedDocEntry(BusinessObjectInfo pVal)
        {
            var docEntryXml = pVal.ObjectKey.ToString();
            XDocument xmlDoc = XDocument.Parse(docEntryXml);
            string docEntry = xmlDoc.Root.Element("DocEntry")?.Value;

            return docEntry;
        }
    }
}
