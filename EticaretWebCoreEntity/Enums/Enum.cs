using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Enums
{
    public enum DosyaYoluTipleri : int
    {
        [Display(Name = "Resim")] Resim = 1,
        [Display(Name = "Dosya")] Dosya = 2,
        [Display(Name = "Breadcumb")] Breadcumb = 3,
    }
    public enum PaytrApiModu : int
    {
        [Display(Name = "Canlı")] Canli = 0,
        [Display(Name = "Test")] Test = 1,
    }
    public enum ZiraatPayOdemeDurumu : int
    {
        [Display(Name = "Ödeme Başarısız")] Basarisiz = 1,
        [Display(Name = "Ödeme Başarılı")] Basarili = 2,
    }

    public enum KuponOranTipi : int
    {
        [Display(Name = "Yüzde")] Yuzde = 0,
        [Display(Name = "Sabit")] Sabit = 1,
    }

    public enum ColumnSayisi : int
    {
        [Display(Name = "1")] Bir = 1,
        [Display(Name = "2")] iki = 2,
        [Display(Name = "3")] Uc = 3,
        [Display(Name = "4")] Dort = 4,
        [Display(Name = "6")] Alti = 6,

    }

    public enum Device : int
    {
        [Display(Name = "Mobil")] Mobil = 1,
        [Display(Name = "Tablet")] Tablet = 2,
        [Display(Name = "Desktop Medium")] DesktopMedium = 3,
        [Display(Name = "Desktop Large")] DesktopLarge = 4,

    }


    public enum FiyatTipleri : int
    {
        [Display(Name = "Liste Fiyatı")] ListeFiyat = 1,
        [Display(Name = "Bayi Fiyatı")] BayiFiyat = 2,
        [Display(Name = "Size Özel Fiyat")] SizeOzelFiyat = 3,
    }

    public enum OpakOdemeYontemi : int
    {
        [Display(Name = "Ziraat Pay Kredi Kartı")] ZiraatPayKrediKarti = 13,
        [Display(Name = "Web Havale")] WebHavale = 15,
        [Display(Name = "Web Açık Çek")] WebAcikCek = 14,
    }

    public enum UyeKayitTipi : int
    {
        [Display(Name = "Web")] Web = 1,
        [Display(Name = "Opak")] Opak = 2,
    }
}
