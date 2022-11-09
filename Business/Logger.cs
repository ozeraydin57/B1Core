using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Business
{
    public static class Logger
    {

        public static void WriteErrorLog(string message, string fileName = "Log")
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Logs"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Logs");
                string db = "";
                string user = "";
                try
                {
                    db = Main.oCompany.CompanyDB.ToString();
                    user = Main.oCompany.UserName.ToString();
                }
                catch
                {

                }

                StreamWriter sw = null;
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Logs\\LogFile_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + db + "_" + user + "_" + fileName + ".txt", true);
                sw.WriteLine(DateTime.Now + ": " + message);
                sw.Flush();
                sw.Close();
                sw = null;
            }
            catch
            {

            }
        }
    }
}
