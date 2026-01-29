using EticaretWebCoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EticaretWebCoreHelper
{
    public static class EmailDogrulamaHelper
    {
        public static void MailGonder(string link, string email)
        {
            AppDbContext _context = new();

            var siteAyari = _context.SiteAyarlari.ToList().FirstOrDefault();

            MailMessage mail = new();
         
            mail.From = new MailAddress(siteAyari?.EmailAdresi);
            mail.To.Add(email);
            mail.Subject = $"Email Doğrulama";
            mail.Body = "<h2>Email adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız</h2><hr/>";
            mail.Body += $"<a href='{link}'>Email Doğrulama Linki</a>";
            mail.IsBodyHtml = true;


            //smtpClient.Credentials = new System.Net.NetworkCredential("info@areltasarim.com", "Areltasarim3429");

           SmtpClient smtp = new(siteAyari?.EmailHost, siteAyari.EmailPort);
            smtp.Credentials = new NetworkCredential(siteAyari?.EmailAdresi, siteAyari?.EmailSifre);

            smtp.EnableSsl = siteAyari.EmailSSL;



            smtp.Send(mail);
        }
    }
}
