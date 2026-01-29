using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum AdresKayitTipleri : int
    {
        [Display(Name = "Üye Ol Sayfa")] UyeOlSayfa = 0,
        [Display(Name = "Modal")] Modal = 1
    }
}
