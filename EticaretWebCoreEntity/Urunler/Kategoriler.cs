using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Kategoriler : BaseEntity
    {

        public override string ToString()
        {
            return this.KategorilerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR")?.KategoriAdi;
        }

        public int ParentKategoriId { get; set; }
        public string Resim { get; set; }
        public string BreadcrumbResim { get; set; }
        public string Ikon { get; set; }

        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public SayfaDurumlari Vitrin { get; set; }
        public virtual ICollection<KategorilerTranslate> KategorilerTranslate { get; set; } = new List<KategorilerTranslate>();

        public virtual Kategoriler ParentKategori { get; set; }
        public virtual ICollection<Kategoriler> AltKategoriler { get; set; }
        public virtual ICollection<UrunToKategori> UrunToKategori { get; set; }
        public virtual ICollection<Sayfalar> Sayfalar { get; set; }
        public virtual ICollection<OneCikanKategoriToKategoriler> ModulToOneCikanKategori { get; set; }
        public virtual ICollection<KategoriBanner> KategoriBanner { get; set; }

        public virtual ICollection<UrunToBenzerUrun> UrunToBenzerUrun { get; set; }

        public virtual ICollection<OneCikanUrunToKategoriler> ModulToOneCikanUrunKategori { get; set; }


        //public virtual ICollection<Menuler> Menuler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Kategoriler>(entity =>
            {

                entity
                .HasMany(p => p.KategorilerTranslate)
                .WithOne(p => p.Kategoriler)
                .HasForeignKey(p => p.KategoriId)
                .OnDelete(DeleteBehavior.Cascade);

            });


            builder.Entity<Kategoriler>(entity => {
                entity.HasMany(p => p.AltKategoriler)
                .WithOne(p => p.ParentKategori)
                .HasForeignKey(p => p.ParentKategoriId)
                .OnDelete(DeleteBehavior.Restrict);
            });

           
        }

    }

    public class KategorilerTranslate : BaseEntity
    {

        public string KategoriAdi { get; set; }

        public string KisaAciklama { get; set; }

        public string Aciklama { get; set; }
        public string BreadcrumbAdi { get; set; }
        public string BreadcrumbAciklama { get; set; }

        //
        public string UstAciklama { get; set; }
        public string SolAciklama { get; set; }
        public string AltAciklama { get; set; }

        //

        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int KategoriId { get; set; }
        public virtual Kategoriler Kategoriler { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<KategorilerTranslate>(entity =>
            {
                entity
                .Property(p => p.KategoriAdi)
                .HasMaxLength(255);
            });
        }
    }
}