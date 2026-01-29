using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class FotografGalerileri : BaseEntity
    {

        public GaleriTipleri GaleriTipi { get; set; }

        //NOT : TOPLU ÜRÜN YÜKLERKEN HANGİ KLASÖRE TOPLU RESİM YÜKLEDİĞİNİ AYIRT ETMEK İÇİN, ÜRÜN,KATEGORİ,MARKA VS.
        public GaleriSayfaTipleri GaleriSayfaTipi { get; set; }
        public string Resim { get; set; }
        public virtual ICollection<FotografGalerileriTranslate> FotografGalerileriTranslate { get; set; }
        public virtual ICollection<FotografGaleriResimleri> FotografGaleriResimleri { get; set; }
        public SayfaDurumlari SilmeYetkisi { get; set; }
        public SayfaDurumlari AdminSolMenu { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FotografGalerileri>(entity =>
            {
                entity
                .HasMany(p => p.FotografGalerileriTranslate)
                .WithOne(p => p.FotografGalerileri)
                .HasForeignKey(p => p.FotografGaleriId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<FotografGalerileri>(entity =>
            {

                entity
                .HasMany(p => p.FotografGaleriResimleri)
                .WithOne(p => p.FotografGalerileri)
                .HasForeignKey(p => p.FotografGaleriId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }

    }

    public class FotografGalerileriTranslate : BaseEntity
    {

        public string GaleriAdi { get; set; }
        public string KisaAciklama { get; set; }
        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int FotografGaleriId { get; set; }
        public virtual FotografGalerileri FotografGalerileri { get; set; }

        public override void Build(ModelBuilder builder)
        {

            builder.Entity<FotografGalerileriTranslate>(entity =>
            {
                entity
               .Property(p => p.GaleriAdi)
               .HasMaxLength(255);
            });

        }
    }

}