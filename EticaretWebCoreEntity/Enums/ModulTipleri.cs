using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum ModulTipleri : int
    {
        [Display(Name = "Öne Çıkan Kategoriler")] OneCikanKategoriler = 1,
        [Display(Name = "Tab Ürünler")] TabUrunler = 2,
        [Display(Name = "Öne Çıkan Ürünler")] OneCikanUrunler = 3,
    }


    public enum OneCikanUrunTipleri : int
    {
        [Display(Name = "Öne Çıkan Ürünler")] OneCikanUrunler = 1,
        [Display(Name = "Üst Menü Ürünler")] UstMenuUrunler = 2,
        [Display(Name = "Arama Bölümü Ürünler")] SearchUrunler = 3,
    }
}
