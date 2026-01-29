using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum GaleriSayfaTipleri : int
    {
        [Display(Name = "Sayfa")] Sayfa,
        [Display(Name = "Marka")] Marka,
        [Display(Name = "Ürün")] Urun,
        [Display(Name = "Kategori")] Kategori,
    }
}
