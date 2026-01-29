using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{

    public enum SeoUrlTipleri
    {
        [Display(Name = "Dinamik Sayfa Detay")] DinamikSayfaDetay = 10,
        [Display(Name = "Hakkımızda")] Hakkimizda = 15,
        [Display(Name = "Url")] Url = 35,
        [Display(Name = "Galeri")] Galeri = 45,
        [Display(Name = "Marka")] Marka = 50,
        [Display(Name = "Kategori")] Kategori = 60,
        [Display(Name = "Ürün")] Urun = 70,
        [Display(Name = "Tüm Markalar")] Markalar = 75,
        [Display(Name = "Tüm Ürünler")] Urunler = 130,
        [Display(Name = "Tüm Kategoriler")] Kategoriler = 140,
        [Display(Name = "Referanslar")] Referanslar = 150,
        [Display(Name = "Projeler")] Projeler = 160,
        [Display(Name = "Hizmetlerimiz")] Hizmetlerimiz = 170,
        [Display(Name = "Çözümlerimiz")] Cozumlerimiz = 171,
        [Display(Name = "Banner")] Banner = 172,
        [Display(Name = "Duyurular")] Duyurular = 174,
        [Display(Name = "Belgelerimiz")] Belgelerimiz = 175,
        [Display(Name = "Kısa Bilgiler")] KisaBilgiler = 176,
        [Display(Name = "Slogan 1")] Slogan1 = 177,
        [Display(Name = "HeaderAcilklama")] HeaderAcilklama = 178,
        [Display(Name = "Şartname")] Sartname = 179,
        [Display(Name = "SSS")] SSS = 180,
        [Display(Name = "Blog")] Blog = 181,
        [Display(Name = "Download")] Download = 182,




        [Display(Name = "Ana Sayfa Sabit Menü")] AnaSayfaSabitMenu = 100,
        [Display(Name = "Ekibimiz Sabit Menü")] EkibimizSabitMenu = 102,
        [Display(Name = "Referanslar Sabit Menü")] ReferanslarSabitMenu = 150,
        [Display(Name = "Video Galeri Sabit Menü")] VideoGaleriSabitMenu = 152,
        [Display(Name = "Video Kategorileri")] VideoKategorileri = 154,
        [Display(Name = "Video")] Video = 156,
        [Display(Name = "Projeler Sabit Menü")] ProjelerSabitMenu = 160,
        [Display(Name = "Hizmetlerimiz Sabit Menü")] HizmetlerimizSabitMenu = 170,
        [Display(Name = "Çözümlerimiz")] CozumlerimizSabitMenu = 171,
        [Display(Name = "Banner Sabit Menü")] BannerSabitMenu = 172,
        [Display(Name = "Duyurular")] DuyurularSabitMenu = 174,
        [Display(Name = "Belgelerimiz")] BelgelerimizSabitMenu = 175,
        [Display(Name = "Kısa Bilgiler")] KisaBilgilerSabitMenu = 176,
        [Display(Name = "Slogan 1 Sabit Menü")] Slogan1SabitMenu = 177,
        [Display(Name = "HeaderAcilklama Sabit Menü")] HeaderAcilklamaSabitMenu = 178,
        [Display(Name = "Şartname")] SartnameSabitMenu = 179,
        [Display(Name = "SSS Sabit Menü")] SSSabitMenu = 180,
        [Display(Name = "Blog Sabit Menü")] BlogSabitMenu = 181,
        [Display(Name = "Galeri Kategori Sabit Menü")] GaleriKategoriSabitMenu = 190,
        [Display(Name = "İnsan Kaynakları Sabit Menü")] InsanKaynaklariSabitMenu = 195,
        [Display(Name = "Kullanım Kılavuzları")] KullanimKilavuzlari = 197,
        [Display(Name = "Bize Ulaşın Sabit Menü")] BizeUlasinSabitMenu = 200,
        [Display(Name = "Download Sabit Menü")] DownloadSabitMenu = 201,

        [Display(Name = "EKatalog")] EKatalog = 310,


        [Display(Name = "Cerez")] Cerez = 410,
        [Display(Name = "Bayilerimiz")] Bayilerimiz = 411,

        [Display(Name = "Haberler")] Haberler = 501,
        [Display(Name = "Slogan 2 Sabit Menü")] Slogan2SabitMenu = 502,
        [Display(Name = "Slogan 3 Sabit Menü")] Slogan3 = 503,

        [Display(Name = "Slogan 4 Sabit Menü")] Slogan4 = 504,
        [Display(Name = "Yorumlar")] Yorumlar = 505,

        [Display(Name = "Gizlilik İlkeleri")] GizlilikIlkeleri = 506,
        [Display(Name = "Neler Yapıyoruz Sabit Menü")] NelerYapiyoruz = 508,
        [Display(Name = "Kısa Bilgiler 2")] KisaBilgiler2 = 509,
        [Display(Name = "Bölgelerimiz")] Bolgelerimiz = 510,
        [Display(Name = "Teklif Formu")] TeklifFormu = 511,
        [Display(Name = "Sözlük")] Sozluk = 512,

        [Display(Name = "Kvkk")] Kvkk = 513,
        [Display(Name = "Araç Portföyü")] AracPortfoyu = 514,



        //Not buradaki Kisim ile SabitSayfaTipleri enum
        //listesindeki verilerin uyusmasi gerekiyor
    }


    public enum SabitSayfaTipleri
    {
        [Display(Name = "Ana Sayfa")] AnaSayfa = 100,
        [Display(Name = "Ekibimiz")] Ekibimiz = 102,
        [Display(Name = "Video Galeri")] VideoGaleri = 104,
        [Display(Name = "Marka")] Marka = 50,
        [Display(Name = "Markalar")] Markalar = 75,
        [Display(Name = "Ürünler")] Urunler = 130,
        [Display(Name = "Kategoriler")] Kategoriler = 140,
        [Display(Name = "Referanslar")] Referanslar = 150,
        [Display(Name = "Video Galeri SabitMenu")] VideoGaleriSabitMenu = 152,
        [Display(Name = "Video Kategorileri")] VideoKategorileri = 154,
        [Display(Name = "Video")] Video = 156,
        [Display(Name = "Projeler")] Projeler = 160,
        [Display(Name = "Hizmetlerimiz")] Hizmetlerimiz = 170,
        [Display(Name = "Çözümlerimiz")] Cozumlerimiz = 171,
        [Display(Name = "Banner")] Banner = 172,
        [Display(Name = "Duyurular")] Duyurular = 174,
        [Display(Name = "Belgelerimiz")] Belgelerimiz = 175,
        [Display(Name = "Kısa Bilgiler")] KisaBilgiler = 176,
        [Display(Name = "Sloganlar")] Sloganlar = 177,
        [Display(Name = "HeaderAcilklama")] HeaderAcilklama = 178,
        [Display(Name = "Şartname")] Sartname = 179,
        [Display(Name = "Download")] Download = 182,

        [Display(Name = "Sık Sorulan Sorular")] SSS = 180,
        [Display(Name = "Blog")] Blog = 181,
        [Display(Name = "Galeri Kategori")] GaleriKategori = 190,
        [Display(Name = "İnsan Kaynaklari")] InsanKaynaklari = 195,
        [Display(Name = "Kullanım Kılavuzları")] KullanimKilavuzlari = 197,
        [Display(Name = "Bize Ulaşın")] BizeUlasin = 200,


        [Display(Name = "Cerez Sabit Menü")] CerezSabitMenu = 410,
        [Display(Name = "Bayilerimiz Sabit Menü")] BayilerimizSabitMenu = 411,

        [Display(Name = "Haberler")] Haberler = 501,
        [Display(Name = "Slogan 2")] Slogan2 = 502,
        [Display(Name = "Slogan 3")] Slogan3 = 503,

        [Display(Name = "Slogan 4")] Slogan4 = 504,
        [Display(Name = "Yorumlar Sabit Menü")] YorumlarSabitMenu = 505,
        [Display(Name = "Gizlilik İlkeleri")] GizlilikIlkeleriSabitMenu = 506,
        [Display(Name = "Neler Yapıyoruz Sabit Menü")] NelerYapiyoruzSabitMenu = 508,
        [Display(Name = "Kısa Bilgiler 2")] KisaBilgiler2SabitMenu = 509,
        [Display(Name = "Bölgelerimiz")] BolgelerimizSabitMenu = 510,
        [Display(Name = "Teklif Formu")] TeklifFormu = 511,
        [Display(Name = "Sözlük")] Sozluk = 512,
        [Display(Name = "Kvkk")] Kvkk = 513,
        [Display(Name = "Araç Portföyü")] AracPortfoyu = 514,

        //Not buradaki Kisim ile SeoUrlTipleri enum listesindeki verilerin uyusmasi gerekiyor
    }

    public enum SayfaTipleri : int
    {
        [Display(Name = "Dinamik Sayfa")] DinamikSayfa = 10,
        [Display(Name = "Hakkımızda")] Hakkimizda = 15,
        [Display(Name = "Url")] Url = 35,
        [Display(Name = "Galeri")] Galeri = 45,
        [Display(Name = "Marka")] Marka = 50,
        [Display(Name = "Kategori")] Kategori = 60,
        [Display(Name = "Ürün")] Urun = 70,
        [Display(Name = "Markalar")] Markalar = 75,
        [Display(Name = "Ürünler")] Urunler = 130,

        [Display(Name = "Kategoriler")] Kategoriler = 140,
        [Display(Name = "Referanslar")] Referanslar = 150,
        [Display(Name = "Video Galeri SabitMenu")] VideoGaleriSabitMenu = 152,
        [Display(Name = "Video Kategorileri")] VideoKategorileri = 154,
        [Display(Name = "Video")] Video = 156,
        [Display(Name = "Projeler")] Projeler = 160,
        [Display(Name = "Hizmetlerimiz")] Hizmetlerimiz = 170,
        [Display(Name = "Çözümlerimiz")] Cozumlerimiz = 171,
        [Display(Name = "Banner")] Banner = 172,
        [Display(Name = "Duyurular")] Duyurular = 174,
        [Display(Name = "Belgelerimiz")] Belgelerimiz = 175,
        [Display(Name = "Kısa Bilgiler")] KisaBilgiler = 176,
        [Display(Name = "Slogan")] Slogan1 = 177,
        [Display(Name = "HeaderAcilklama")] HeaderAcilklama = 178,
        [Display(Name = "Şartname")] Sartname = 179,
        [Display(Name = "Sık Sorulan Sorular")] SSS = 180,
        [Display(Name = "Blog")] Blog = 181,
        [Display(Name = "Download")] Download = 182,
        [Display(Name = "Galeri Kategori")] GaleriKategori = 190,
        [Display(Name = "İnsan Kaynaklari")] InsanKaynaklari = 195,
        [Display(Name = "Kullanım Kılavuzları")] KullanimKilavuzlari = 197,
        [Display(Name = "Ekibimiz")] Ekibimiz = 102,

        [Display(Name = "Cerez")] Cerez = 410,
        [Display(Name = "Bayilerimiz")] Bayilerimiz = 411,

        [Display(Name = "Haberler")] Haberler = 501,
        [Display(Name = "Slogan 2")] Slogan2 = 502,
        [Display(Name = "Slogan 3")] Slogan3 = 503,

        [Display(Name = "Slogan 4")] Slogan4 = 504,
        [Display(Name = "Yorumlar")] Yorumlar = 505,
        [Display(Name = "Gizlilik İlkeleri")] GizlilikIlkeleri = 506,
        [Display(Name = "Neler Yapıyoruz")] NelerYapiyoruz = 508,
        [Display(Name = "Kısa Bilgiler 2")] KisaBilgiler2 = 509,
        [Display(Name = "Bölgelerimiz")] Bolgelerimiz = 510,
        [Display(Name = "Teklif Formu")] TeklifFormu = 511,
        [Display(Name = "Sözlük")] Sozluk = 512,
        [Display(Name = "Kvkk")] Kvkk = 513,
        [Display(Name = "Araç Portföyü")] AracPortfoyu = 514,



        [Display(Name = "EKatalog")] EKatalog = 310,

        //Not buradaki Kisim ile SeoUrlTipleri enum listesindeki verilerin uyusmasi gerekiyor

    }

    public enum GaleriTipleri : int
    {
        [Display(Name = "Galeri")] Galeri = 45,
        [Display(Name = "EKatalog")] EKatalog = 310,
    }

}
