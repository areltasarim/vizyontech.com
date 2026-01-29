using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity
{
    public class Siparisler : BaseEntity
    {
        public int? UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }

        //public int SiparisDurumId { get; set; }
        //public virtual SiparisDurumlari SiparisDurumlari { get; set; }

        public int? AdresId { get; set; }
        public virtual Adresler Adresler { get; set; }

        //public int OdemeMetodId { get; set; }
        //public virtual OdemeMetodlari OdemeMetodlari { get; set; }

        public DateTime SiparisTarihi { get; set; }
        public DateTime SiparisGuncellemeTarihi { get; set; }
        public string SiparisNo { get; set; }
        public string FaturaAd { get; set; }
        public string FaturaSoyad { get; set; }
        public string FaturaFirmaAdi { get; set; }
        public string FaturaAdres { get; set; }
        public string FaturaUlke { get; set; }
        public string FaturaIl { get; set; }
        public string FaturaIlce { get; set; }
        public string TeslimatAd { get; set; }
        public string TeslimatSoyad { get; set; }
        public string TeslimatFirmaAdi { get; set; }
        public string TeslimatAdres { get; set; }
        public string TeslimatUlke { get; set; }
        public string TeslimatIl { get; set; }
        public string TeslimatIlce { get; set; }
        public string KargoMetodu { get; set; }
        public string OdemeMetodu { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string VergiDairesi { get; set; }
        public long? VergiNumarasi { get; set; }
        public string ParaBirimiKodu { get; set; } 
        public string SiparisNotu { get; set; }

        public string Ip { get; set; }
        public decimal KargoUcreti { get; set; }
        public decimal ToplamFiyat { get; set; }

        public int? ParaBirimId { get; set; }
        public virtual ParaBirimleri ParaBirimleri { get; set; }
        public decimal Kur { get; set; }

        //En Son Güncellenen Sipariş Durumu
        public int SiparisDurumu { get; set; }

        public string KargoKodu { get; set; }

        public string KargoUrl { get; set; }

        public virtual ICollection<SiparisUrunleri> SiparisUrunleri { get; set; }
        public virtual ICollection<SiparisGecmisleri> SiparisGecmisleri { get; set; }
        public virtual ICollection<SiparisUrunSecenekleri> SiparisUrunSecenekleri { get; set; }
        public virtual ICollection<PaytrIframeTransaction> PaytrIframeTransaction { get; set; }
        public virtual ICollection<KuponToSiparis> KuponToSiparis { get; set; }
        
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Siparisler>(entity =>
            {
                entity
                .Property(p => p.KargoUcreti)
                .HasPrecision(18, 4);

                entity
               .Property(p => p.ToplamFiyat)
               .HasPrecision(18, 4);

                entity
                .Property(p => p.Kur)
                .HasPrecision(18, 4);
            });
            //builder.Entity<SiparisDurumlari>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Siparisler)
            //    .WithOne(p => p.SiparisDurumlari)
            //    .HasForeignKey(p => p.SiparisDurumId)
            //    .OnDelete(DeleteBehavior.Restrict);
            //});

            //builder.Entity<OdemeMetodlari>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Siparisler)
            //    .WithOne(p => p.OdemeMetodlari)
            //    .HasForeignKey(p => p.OdemeMetodId)
            //    .OnDelete(DeleteBehavior.Restrict);
            //});
            builder.Entity<Adresler>(entity =>
            {
                entity
                .HasMany(p => p.Siparisler)
                .WithOne(p => p.Adresler)
                .HasForeignKey(p => p.AdresId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.Siparisler)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);
            });



        }
    }
}
