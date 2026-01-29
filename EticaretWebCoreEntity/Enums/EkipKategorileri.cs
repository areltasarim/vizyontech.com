using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum EkipKategorileri : int
    {
        [Display(Name = "Üye")] Uye = 0,
        [Display(Name = "Yönetim Kurulu")] YonetimKurulu = 1
    }
}
