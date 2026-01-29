using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum OdemeMetodTiplieri
    {
        [Display(Name = "Banka Havalesi")] BankaHavalesi = 1,
        [Display(Name = "Kapıda Ödeme")] KapidaOdeme = 2,
        [Display(Name = "Mağazadan Teslim Al")] MagazadanTeslimAl = 3,
        [Display(Name = "Paytr")] Paytr = 4,
        [Display(Name = "İyzico")] Iyzico = 5,
        [Display(Name = "ZiraatPay")] ZiraatPay = 6,
        [Display(Name = "Cari Hesabıma Yaz")] CariHesabimaYaz = 7,
    }
}
