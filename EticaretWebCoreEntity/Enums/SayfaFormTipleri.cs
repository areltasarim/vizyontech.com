using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum SayfaFormTipleri : int
    {
        [Display(Name = "İletişim")] Iletisim,
        [Display(Name = "Abone")] Abone,
        [Display(Name = "Ürün")] Urun,
        [Display(Name = "Rezarvasyon")] Rezarvasyon
    }
}
