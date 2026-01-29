using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Sayfalar : BaseEntity
    {
        public override string ToString()
        {
            return this.SayfalarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR")?.SayfaAdi;
        }


        public int ParentSayfaId { get; set; }
        public SayfaTipleri SayfaTipi { get; set; }
        public SSSDurumu SSS { get; set; }
        public SeoUrlTipleri EntityName { get; set; }
        public int Hit { get; set; }
        public string BreadcrumbResim { get; set; }

        public virtual Sayfalar ParentSayfa { get; set; }
        public virtual ICollection<Sayfalar> AltSayfalar { get; set; }
        public SayfaDurumlari SinirsizAltSayfaDurumu { get; set; }
        public virtual ICollection<SayfalarTranslate> SayfalarTranslate { get; set; } = new List<SayfalarTranslate>();
        public virtual ICollection<SayfaResimleri> SayfaResimleri { get; set; }
        public virtual ICollection<FormBasliklari> FormBasliklari { get; set; }
        public virtual ICollection<Yorumlar> Yorumlar { get; set; }

        public virtual ICollection<SayfaToSayfalar> SayfaToSayfalar { get; set; }
        public virtual ICollection<SayfaToOzellik> SayfaToOzellik { get; set; }

        public int? UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }

        public SayfaDurumlari SilmeYetkisi { get; set; }
        public AdminSolMenuDurumlari AdminSolMenu { get; set; }
        public string KisayolMenuAdi { get; set; }

        public DateTime Tarih { get; set; }
        public string Ikon { get; set; }
        public string Ikon2 { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public SayfaDurumlari Vitrin { get; set; }

        [Display(Name = "Sıra")]
        [Required(ErrorMessage = "{0} Boş Bırakılamaz")]
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Sayfalar>(entity =>
            {
                entity
                .HasMany(p => p.SayfalarTranslate)
                .WithOne(p => p.Sayfalar)
                .HasForeignKey(p => p.SayfaId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Sayfalar>(entity =>
            {
                entity.HasMany(p => p.AltSayfalar)
                .WithOne(p => p.ParentSayfa)
                .HasForeignKey(p => p.ParentSayfaId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Sayfalar>(entity =>
            {

                entity
                .HasMany(p => p.SayfaResimleri)
                .WithOne(p => p.Sayfalar)
                .HasForeignKey(p => p.SayfaId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<AppUser>(entity =>
            {

                entity
                .HasMany(p => p.Sayfalar)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }

    }

    public class SayfalarTranslate : BaseEntity
    {

        public string SayfaAdi { get; set; }
        public string SayfaAdiAltAciklama { get; set; }
        public string SayfaAdiAltAciklama2 { get; set; }
        public string KisaAciklama { get; set; }
        public string Aciklama { get; set; }
        public string YoutubeVideoLink { get; set; }
        public string BreadcrumbAdi { get; set; }
        public string BreadcrumbAciklama { get; set; }
        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }
        public string ButonAdi { get; set; }
        public string ButonUrl { get; set; }
        public string Resim { get; set; }
        public string Resim2 { get; set; }
        public string Dosya { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }


        public int SayfaId { get; set; }
        public virtual Sayfalar Sayfalar { get; set; }

        [NotMapped]
        public IFormFile SayfaResmi { get; set; }
        [NotMapped]
        public IFormFile SayfaResmi2 { get; set; }
        [NotMapped]
        public IFormFile SayfaDosyasi { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfalarTranslate>(entity =>
            {
                entity
               .Property(p => p.SayfaAdi)
               .HasMaxLength(750);
            });

        }
    }

}