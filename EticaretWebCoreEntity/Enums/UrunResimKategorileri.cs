using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum UrunResimKategorileri : int
    {
        [Display(Name = "Resimler")] UrunResim,
        [Display(Name = "Ürün Dosya")] UrunDosya,
        [Display(Name = "Kullanım Kılavuzları")] KullanimKulavuzu,
        [Display(Name = "Şartname")] Sartname,
        [Display(Name = "Belge")] Belge,
        [Display(Name = "Video")] Video,
    }
}
