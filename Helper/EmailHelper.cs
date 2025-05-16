using B1Core.Business;
using B1Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace B1Core.Helper
{
    public static class EmailHelper
    {
        public static Response<bool> SendEmailBildirim(string email, string konu, string content,string link, EmailSMTPInfo emailInfo, List<string> bcc = null)
        {
            var resp = new Response<bool>();
            try
            {
                string html = "";

                html += $" <b>Merhaba</b> <br />";
                html += content + "<br />";
                html += $"<a href='{link}'> Teklif Vermek için Tıklayınız.</a>";

                SendEmail(email, konu, html, emailInfo, bcc);
                resp.Success = true;
                Logger.WriteErrorLog("Eposta gönderildi ");
            }
            catch (Exception ex)
            {
                var err = ex.Message.Replace('\'', ' ').Replace('\\', ' ').Replace(',', ' ');
                resp.Message = err.Length > 240 ? err.Substring(0, 240) : ex.Message;
                Logger.WriteErrorLog($"Eposta gönderilemedi {email} , " + ex.ToString());
            }

            return resp;
        }
        private static void SendEmail(string sendEmail, string konu, string html, EmailSMTPInfo emailInfo, List<string> bcc)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(emailInfo.FromMail);
            message.To.Add(new MailAddress(sendEmail));
            if (bcc != null && bcc.Count > 0)
            {
                foreach (var item in bcc)
                {
                    message.Bcc.Add(item);
                }
            }

            message.Subject = konu;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = html;
            message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient(emailInfo.Host, int.Parse(emailInfo.Port));
            smtp.Timeout = 20000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = emailInfo.EnableSsl;

            smtp.UseDefaultCredentials = emailInfo.UseDefaultCredentials;
            smtp.Credentials = new NetworkCredential(emailInfo.FromMail, emailInfo.Password);

            smtp.Send(message);
        }

        public static Response<EmailSMTPInfo> GetEmailSMTPInfo(string table)
        {
            var info = new Response<EmailSMTPInfo>();

            var mailinfo = new EmailSMTPInfo();

            mailinfo.FromMail = Data.ReadSingleData(table, "U_Value", "Name", "FromMail").Data.ToString();
            mailinfo.Password = Data.ReadSingleData(table, "U_Value", "Name", "Password").Data.ToString();
            mailinfo.Port = Data.ReadSingleData(table, "U_Value", "Name", "Port").Data.ToString();
            mailinfo.Host = Data.ReadSingleData(table, "U_Value", "Name", "Host").Data.ToString();
            mailinfo.EnableSsl = Data.ReadSingleData(table, "U_Value", "Name", "EnableSsl").Data.ToString() == "1" ? true : false;
            mailinfo.UseDefaultCredentials = Data.ReadSingleData(table, "U_Value", "Name", "UseDefaultCredentials").Data.ToString() == "1" ? true : false;

            info.Data = mailinfo;

            return info;
        }
    }

    public class EmailSMTPInfo
    {
        public bool EnableSsl { get; set; }
        public string FromMail { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }

}