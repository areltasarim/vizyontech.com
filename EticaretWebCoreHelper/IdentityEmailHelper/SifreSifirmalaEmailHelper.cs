using EticaretWebCoreEntity;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace EticaretWebCoreHelper
{
    public static class SifreSifirmalaEmailHelper
    {
        public static void PasswordResetSendEmail(string link, string email)
        {
            AppDbContext _context = new();

            var siteAyari = _context.SiteAyarlari.ToList().FirstOrDefault();

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(siteAyari?.EmailAdresi);
            mail.To.Add(email);
            mail.Subject = $"Şifre Sıfırlama";
            mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız</h2><hr/>";
            mail.Body += $"<a href='{link}'>Şifre Yenileme Linki</a>";
            mail.IsBodyHtml = true;


            SmtpClient smtp = new SmtpClient(siteAyari?.EmailHost, siteAyari.EmailPort);
            smtp.Credentials = new System.Net.NetworkCredential(siteAyari?.EmailAdresi, siteAyari?.EmailSifre);

            smtp.EnableSsl = true;



            smtp.Send(mail);
        }
    }
}
