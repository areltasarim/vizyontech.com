using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EticaretWebCoreEntity
{
    public class PaytrIframeTransaction : BaseEntity
    {
        public string MerchantOid { get; set; }
        public int SiparisId { get; set; }
        public virtual Siparisler Siparis { get; set; }

        //Siparişin toplam Tutarı
        public decimal ToplamFiyat { get; set; }

        //Eğer taksitli bir ödeme veya kısmi bir ödeme yapıldıysa buraya o tutarı yazıyoruz
        public decimal OdenenTutar { get; set; }

        public decimal IadeEdilenTutar { get; set; }
        public DateTime IadeEdilenTarih { get; set; }

        //Eğer İade edilen bir tutar varsa ise Evet yoksa Hayır bilgisi gelecek
        public bool IadeDurumu { get; set; }

        //İade Edilen Tutarın Tamamımı İade Edildi Yoksa Kısmi İademi (Tamamı veya Kismi Yazısı gelecek)
        public string IadeEdilenDurum { get; set; }

        //Paytr ödeme başarılımı değilmi başarılı ise "success" değilse boş değer
        public string Status { get; set; }

        //Eğer ödeme hatalı ise hata mesajı gelecek
        public string StatusMessage { get; set; }
        public DateTime EklemeTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<PaytrIframeTransaction>(entity =>
            {
                entity
                .Property(p => p.ToplamFiyat)
                .HasPrecision(18, 4);

                entity
                  .Property(p => p.OdenenTutar)
                  .HasPrecision(18, 4);

                entity
                  .Property(p => p.IadeEdilenTutar)
                  .HasPrecision(18, 4);
            });

            builder.Entity<Siparisler>(entity =>
            {
                entity
                .HasMany(p => p.PaytrIframeTransaction)
                .WithOne(p => p.Siparis)
                .HasForeignKey(p => p.SiparisId)
                .OnDelete(DeleteBehavior.Cascade);
            });
         
        }
    }

}
