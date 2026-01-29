using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EticaretWebCoreEntity.DilCeviri;

namespace EticaretWebCoreEntity
{
    public class Diller : BaseEntity
    {
        public virtual ICollection<KategorilerTranslate> KategorilerTranslate { get; set; }
        public virtual ICollection<KategoriBanner> KategoriBanner { get; set; }
        public virtual ICollection<UrunlerTranslate> UrunlerTranslate { get; set; }
        public virtual ICollection<UrunSecenekleriTranslate> UrunSecenekleriTranslate { get; set; }
        public virtual ICollection<UrunSecenekDegerleriTranslate> UrunSecenekDegerleriTranslate { get; set; }
        public virtual ICollection<UrunOzellikGruplariTranslate> UrunOzellikGruplariTranslate { get; set; }
        public virtual ICollection<UrunOzellikleriTranslate> UrunOzellikleriTranslate { get; set; }
        public virtual ICollection<UrunToOzellik> UrunToOzellik { get; set; }
        public virtual ICollection<DosyaKategorileriTranslate> DosyaKategorileriTranslate { get; set; }
        public virtual ICollection<DosyalarTranslate> DosyalarTranslate { get; set; }
        public virtual ICollection<MenulerTranslate> MenulerTranslate { get; set; }
        public virtual ICollection<SabitMenulerTranslate> SabitMenulerTranslate { get; set; }
        public virtual ICollection<SayfalarTranslate> SayfalarTranslate { get; set; }
        public virtual ICollection<SayfaOzellikGruplariTranslate> SayfaOzellikGruplariTranslate { get; set; }
        public virtual ICollection<SayfaOzellikleriTranslate> SayfaOzellikleriTranslate { get; set; }
        public virtual ICollection<SayfaToOzellik> SayfaToOzellik { get; set; }
        public virtual ICollection<SayfaResimleri> SayfaResimleri { get; set; }
        public virtual ICollection<OdemeMetodlariTranslate> OdemeMetodlariTranslate { get; set; }
        public virtual ICollection<KargoMetodlariTranslate> KargoMetodlariTranslate { get; set; }
        public virtual ICollection<SiparisDurumlariTranslate> SiparisDurumlariTranslate { get; set; }
        public virtual ICollection<TakvimTranslate> TakvimTranslate { get; set; }
        public virtual ICollection<SlaytlarTranslate> SlaytlarTranslate { get; set; }
        public virtual ICollection<BannerTranslate> BannerTranslate { get; set; }
        public virtual ICollection<BannerResimTranslate> BannerResimTranslate { get; set; }
        public virtual ICollection<FotografGalerileriTranslate> FotografGalerileriTranslate { get; set; }
        public virtual ICollection<VideoKategorileriTranslate> VideoKategorileriTranslate { get; set; }
        public virtual ICollection<VideolarTranslate> VideolarTranslate { get; set; }
        public virtual ICollection<FormBasliklariTranslate> FormBasliklariTranslate { get; set; }
        public virtual ICollection<FormlarTranslate> FormlarTranslate { get; set; }
        public virtual ICollection<FormDegerleriTranslate> FormDegerleriTranslate { get; set; }
        public virtual ICollection<EkiplerTranslate> EkiplerTranslate { get; set; }
        public virtual ICollection<DilCeviriTranslate> DilCeviriTranslate { get; set; }
        public virtual ICollection<ModullerTranslate> ModullerTranslate { get; set; }
        public virtual ICollection<OneCikanKategorilerTranslate> OneCikanKategorilerTranslate { get; set; }
        public virtual ICollection<OneCikanUrunlerTranslate> OneCikanUrunlerTranslate { get; set; }
        public virtual ICollection<OneCikanUrunResimleri> OneCikanUrunResimleri { get; set; }

        public virtual ICollection<Paytr> Paytr { get; set; }


        public virtual ICollection<SeoUrl> SeoUrl { get; set; }
        public virtual ICollection<AdresBilgileriTranslate> AdresBilgileriTranslate { get; set; }
        public virtual ICollection<AdresBilgileriTelefonlarTranslate> AdresBilgileriTelefonlarTranslate { get; set; }
        public virtual ICollection<SiteAyarlari> SiteAyarlari { get; set; }
        public virtual ICollection<SiteAyarlariTranslate> SiteAyarlariTranslate { get; set; }


        public string DilAdi { get; set; }
        public string KisaDilAdi { get; set; }

        public string Resim { get; set; }

        public int Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }

        public int DilKoduId { get; set; }
        public virtual DilKodlari DilKodlari  { get; set; }



        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.DilCeviriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.KategorilerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.KategoriBanner)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.UrunlerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.UrunSecenekleriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.UrunSecenekDegerleriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.UrunOzellikGruplariTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.UrunOzellikleriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.UrunToOzellik)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.OdemeMetodlariTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.KargoMetodlariTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SiparisDurumlariTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.DosyaKategorileriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.DosyalarTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.ModullerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.OneCikanKategorilerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.OneCikanUrunlerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.MenulerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SabitMenulerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SayfalarTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SayfaOzellikGruplariTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SayfaOzellikleriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SayfaToOzellik)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SayfaResimleri)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.OneCikanUrunResimleri)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.TakvimTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.FotografGalerileriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.VideoKategorileriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.VideolarTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SlaytlarTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.BannerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.BannerResimTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.FormBasliklariTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.FormlarTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.FormDegerleriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
      
      
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.EkiplerTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SiteAyarlari)
                .WithOne(p => p.AktifDil)
                .HasForeignKey(p => p.AktifDilId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SiteAyarlariTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.AdresBilgileriTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.AdresBilgileriTelefonlarTranslate)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<Diller>(entity =>
            {
                entity
                .HasMany(p => p.SeoUrl)
                .WithOne(p => p.Diller)
                .HasForeignKey(p => p.DilId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Diller>(entity =>
            {
                entity
               .Property(p => p.DilAdi)
               .HasMaxLength(100);

            });


        }
    }

    public class DilKodlari : BaseEntity
    {
        //public int Id { get; set; }

        [Display(Name = "Dil Kodu")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "{0} en fazla 4 karakter olabilir")]
        public string DilKodu { get; set; }
        public virtual ICollection<Diller> Diller { get; set; }
        public virtual ICollection<ParaBirimleri> ParaBirimi { get; set; }
 

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<DilKodlari>(entity =>
            {
                entity
                .HasMany(p => p.Diller)
                .WithOne(p => p.DilKodlari)
                .HasForeignKey(p => p.DilKoduId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<DilKodlari>(entity =>
            {
                entity
               .Property(p => p.DilKodu)
               .HasMaxLength(5);

            });


            builder.Entity<DilKodlari>(entity =>
            {
                entity
                .HasMany(p => p.ParaBirimi)
                .WithOne(p => p.DilKodu)
                .HasForeignKey(p => p.DilKoduId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}