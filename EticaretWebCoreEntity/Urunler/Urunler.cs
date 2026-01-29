using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Urunler : BaseEntity
    {
        public int OzelFiyatStokSarti { get; set; }

        public DateTime Tarih { get; set; }
        public string UrunKodu { get; set; }
        public decimal ListeFiyat { get; set; }
        //public decimal IndirimliFiyat { get; set; }
        public decimal SizeOzelFiyat { get; set; }
        public StokTipleri StokTipi { get; set; }
        public int Stok { get; set; }
        public string BreadcrumbResim { get; set; }
        public int Sira { get; set; }
        public SayfaDurumlari Vitrin { get; set; }
        public SayfaDurumlari Durum { get; set; }

        public virtual ICollection<UrunlerTranslate> UrunlerTranslate { get; set; } = new List<UrunlerTranslate>();

        public virtual ICollection<UrunToKategori> UrunToKategori { get; set; }
        public virtual ICollection<UrunToUrunSecenek> UrunToUrunSecenek { get; set; }
        public virtual ICollection<UrunToUrunSecenekToUrunDeger> UrunToUrunSecenekToUrunDeger { get; set; }
        public virtual ICollection<SiparisUrunSecenekleri> SiparisUrunSecenekleri { get; set; }

        public int? KdvId { get; set; }
        public virtual Kdv Kdv { get; set; }

        public int? DataSheetId { get; set; }
        public virtual Dosyalar DataSheet { get; set; }

        public virtual ICollection<AlisverisListem> AlisverisListem { get; set; }
        public virtual ICollection<MesajKonulari> MesajKonulari { get; set; }
        public virtual ICollection<Sepet> Sepet { get; set; }
        public virtual ICollection<SiparisUrunleri> SiparisUrunleri { get; set; }
        public virtual ICollection<Yorumlar> Yorumlar { get; set; }

        public int? MarkaId { get; set; }
        public virtual Markalar Markalar { get; set; }

        public virtual ICollection<UrunResimleri> UrunResimleri { get; set; }
        public virtual ICollection<UrunToSlayt> UrunToSlayt { get; set; }
        public virtual ICollection<UrunToOzellik> UrunToOzellik { get; set; }

        public virtual ICollection<OneCikanUrunToUrunler> ModulToOneCikanUrun { get; set; }

        public virtual ICollection<Begeniler> Begeniler { get; set; }


        public virtual ICollection<UrunToBenzerUrun> UrunToBenzerUrun { get; set; }
        public virtual ICollection<UrunToTamamlayiciUrun> UrunToTamamlayiciUrun { get; set; }
        public virtual ICollection<KuponToUrun> KuponToUrun { get; set; }


        [NotMapped]
        public int[] SeciliKategoriler { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Urunler>(entity =>
            {

                entity
                .Property(p => p.ListeFiyat)
                .HasPrecision(18, 4);

                entity
                 .Property(p => p.SizeOzelFiyat)
                 .HasPrecision(18, 4);

                entity
                .HasMany(p => p.UrunlerTranslate)
                .WithOne(p => p.Urunler)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Kdv>(entity =>
            {
                entity
                .HasMany(p => p.Urunler)
                .WithOne(p => p.Kdv)
                .HasForeignKey(p => p.KdvId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Dosyalar>(entity =>
            {
                entity
                .HasMany(p => p.DosyaDataSheet)
                .WithOne(p => p.DataSheet)
                .HasForeignKey(p => p.DataSheetId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Urunler>(entity =>
            {

                entity
                .HasMany(p => p.UrunResimleri)
                .WithOne(p => p.Urunler)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Markalar>(entity =>
            {
                entity
                .HasMany(p => p.Urunler)
                .WithOne(p => p.Markalar)
                .HasForeignKey(p => p.MarkaId)
                .OnDelete(DeleteBehavior.Restrict);

            });

        }
    }


    public class UrunlerTranslate : BaseEntity
    {

        public string UrunAdi { get; set; }
        public string KisaAciklama { get; set; }
        public string Ozellik { get; set; }
        public string Aciklama { get; set; }
        public string Aciklama2 { get; set; }
        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }
        public string Resim { get; set; }
        public string YoutubeResim { get; set; }
        public string Video { get; set; }
        public string Dosya { get; set; }
        public string Dosya2 { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        [NotMapped]
        public IFormFile SayfaResmi { get; set; }

        [NotMapped]
        public IFormFile SayfaDosya { get; set; }

        [NotMapped]
        public IFormFile SayfaDosya2 { get; set; }

        [NotMapped]
        public IFormFile BreadcrumbImage { get; set; }

        [NotMapped]
        public IFormFile YoutubeSayfaResim { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunlerTranslate>(entity =>
            {
                entity
               .Property(p => p.UrunAdi)
               .HasMaxLength(500);

            });
        }
    }
}