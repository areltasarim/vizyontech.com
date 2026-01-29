using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum MailTipleri
    {
        [Display(Name = "Host Mail")] HostMail,
        [Display(Name = "Exchange Mail")] ExchangeMail
    }
}
