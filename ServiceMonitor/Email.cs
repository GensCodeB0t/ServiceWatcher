using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace ServiceMonitor
{
    static class Email
    {
        public static void SendMail(string processName)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("gensttestmail@gmail.com");

            // The important part -- configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   // 587 Works for Gmail
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false; 
            smtp.Credentials = new NetworkCredential(mail.From.ToString(), "Marshall83");  // [4] Added this. Note, first parameter is NOT string.
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress("gensy83@gmail.com"));

            //Formatted mail body
            mail.IsBodyHtml = true;
            string st = $"{processName} has stopped working";

            mail.Body = st;
            smtp.Send(mail);
        }
    }
}
