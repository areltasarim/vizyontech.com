using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum MenuTipleri : int
    {
        [Display(Name = "Dinamik Sayfalar")] DinamikSayfalar = 10,
        [Display(Name = "Kategoriler")] Kategoriler = 90,
        [Display(Name = "Ürünler")] Urunler = 80,
        [Display(Name = "Sabit Menü")] SabitMenu = 30,
        [Display(Name = "Dosyalar")] Dosyalar = 31,
        [Display(Name = "Url")] Url = 35,
        [Display(Name = "Galeri")] Galeri = 40,
        [Display(Name = "Marka")] Marka = 50,
        [Display(Name = "EKatalog")] EKatalog = 310,
    }

    public enum MenuSekmeleri : int
    {
        [Display(Name = "Aynı Sekme")] AyniSekme,
        [Display(Name = "Yeni Sekme")] YeniSekme,
    }
}
