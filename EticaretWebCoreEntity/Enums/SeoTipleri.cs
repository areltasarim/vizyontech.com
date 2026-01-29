using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum SeoTipleri : int
    {
        [Display(Name = "Sayfa")] Sayfa = 0,
        [Display(Name = "Marka")] Marka = 1,
        [Display(Name = "Ürün")] Urun = 2,
        [Display(Name = "Tüm Ürünler")] TumUrunler = 3,
        [Display(Name = "Kategori")] Kategori = 4,
        [Display(Name = "Tüm Kategoriler")] TumKategoriler = 5,
        [Display(Name = "Galeri Kategorileri")] GaleriKategorileri = 6,
        [Display(Name = "Galeri")] Galeri = 7,
        [Display(Name = "E-Katalog")] EKatalog = 8,
        [Display(Name = "Bize Ulaşın")] BizeUlasin = 9,
        [Display(Name = "Tüm Markalar")] TumMarkalar = 10,
       
    }
}
