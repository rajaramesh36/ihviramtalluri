namespace InstaHangouts.Common.SendMail
{
    using InstaHangouts.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Class SendMail.
    /// </summary>
    public class SendMail
    {
        /// <summary>
        /// Send mails to the specified  address.
        /// </summary>
        /// <param name="toAddress">To address.</param>
        /// <param name="fromAddress">From address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="dict">The dictionary.</param>
        public void Sendmail(string toAddress, string fromAddress, string subject, UserModel user, string strBody, Dictionary<string, string> dict)
        {   
            SmtpClient client = this.BuildSmtpClient();            
            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userName, password);
            //client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"].ToString());
            ////client.EnableSsl = true;
            //client.Credentials = credentials;
            //client.UseDefaultCredentials = false;
            ////client.TargetName = "STARTTLS/smtp.office365.com";
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            foreach (string address in toAddress.Split(';'))
            {
                mail.To.Add(address);
            }

            mail.From = new MailAddress(fromAddress);
            mail.Subject = subject;
            mail.Body = strBody;
            if (dict.Count != 0)
            {
                foreach (var item in dict)
                {
                    string key = '{' + item.Key + '}';
                    mail.Body = mail.Body.Replace(key, item.Value);
                }
            }
            //string logoPath = HttpContext.Current.Server.MapPath("~");
            //logoPath = logoPath + "\\Content\\images\\logo1.png";
            //var logo = new LinkedResource(logoPath);
            //logo.ContentId = "companylogo";
            AlternateView av1 = AlternateView.CreateAlternateViewFromString(mail.Body, Encoding.UTF8, MediaTypeNames.Text.Html);
            //av1.LinkedResources.Add(logo);
            mail.AlternateViews.Add(av1);           
            client.Send(mail);
        }        

        private SmtpClient BuildSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Host = ConfigurationManager.AppSettings["MailHost"]; 
            var userName = ConfigurationManager.AppSettings["MailUserName"];
            var password = ConfigurationManager.AppSettings["MailPassword"];

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userName, password);
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"].ToString());            
            client.Credentials = credentials;
            return client;
        }
    }
}
