using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum SiparisDurumTipleri
    {
        [Display(Name = "Eksik Sipariş")] EksikSiparis = 1,
        [Display(Name = "Ödeme Bekleniyor")] OdemeBekleniyor = 2,
        [Display(Name = "Siparişiniz Hazırlanıyor")] SiparisinizHazirlaniyor = 3,
        [Display(Name = "Siparişiniz Onaylandı")] SiparisinizOnaylandi = 4,
        [Display(Name = "İşleme Alındı")] IslemeAlindi = 5,
        [Display(Name = "Kargoya Verildi")] KargoyaVerildi = 6,
        [Display(Name = "Ödeme Başarısız")] OdemeBasarisiz = 7,
        [Display(Name = "İptal Edildi")] IptalEdildi = 8,
        [Display(Name = "İade Edildi")] IadeEdildi = 9,
        [Display(Name = "Cari Hesapdan Ödeme")] CariHesapdanOdeme = 10,
    }
}
