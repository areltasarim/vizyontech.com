using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum DosyaTipleri : int
    {
        [Display(Name = "Pdf")] Pdf,
        [Display(Name = "Word")] Word,
    }
    public enum DosyaSayfaTipleri : int
    {
        [Display(Name = "Sayfa")] Sayfa,
        [Display(Name = "Ürün")] Urun,
    }

}
