using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public static class MailHelper
    {
        public static void HostMailGonder(string emailAdresi, string emailSifre, string emailHost, bool emailSSL, int emailPort, string konu, string mailBaslik, string mesaj, List<System.Net.Mail.Attachment> dosya, List<string> gonderilecekMail)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

            mailMessage.From = new System.Net.Mail.MailAddress(emailAdresi, mailBaslik);
            mailMessage.Subject = konu;


            if (dosya != null)
            {
                foreach (var attachment in dosya)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }


            mailMessage.To.Add(string.Join(",", gonderilecekMail.ToArray()));

            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mesaj;



            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(emailHost, emailPort);
            smtp.Credentials = new System.Net.NetworkCredential(emailAdresi, emailSifre);
            smtp.EnableSsl = emailSSL;
         
            smtp.Send(mailMessage);

            return;
        }
        public static void ExchangeMailGonder(string emailAdresi, string emailSifre, string emailHost, ExchangeVersion exchangeVersiyon, string konu, string mesaj, string gonderilecekMail)
        {
            string subject = konu;
            string body = mesaj;
            ExchangeService service = new ExchangeService(exchangeVersiyon);
            service.Credentials = new WebCredentials(emailAdresi, emailSifre);

            service.Url = new Uri(emailHost);

            EmailAddress kime = new EmailAddress(gonderilecekMail);

            EmailMessage message = new EmailMessage(service);
            message.Subject = subject;
            message.Body = body;
            message.ToRecipients.Add(kime);
            message.Save();
            message.SendAndSaveCopy();

            return;
        }

    }
}
