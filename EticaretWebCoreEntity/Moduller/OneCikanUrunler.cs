using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class OneCikanUrunler : BaseEntity
    {
        public virtual ICollection<OneCikanUrunlerTranslate> OneCikanUrunlerTranslate { get; set; } = new List<OneCikanUrunlerTranslate>();
        public virtual ICollection<OneCikanUrunToUrunler> OneCikanUrunToUrunler { get; set; }
        public virtual ICollection<OneCikanUrunToKategoriler> OneCikanUrunToKategoriler { get; set; }
        public virtual ICollection<OneCikanUrunResimleri> OneCikanUrunResimleri { get; set; }


        public int ModulId { get; set; }
        public ColumnSayisi ColumnDesktop { get; set; }
        public ColumnSayisi ColumnMobil { get; set; }
        public string Banner { get; set; }
        public string BannerUrl { get; set; }
        public SayfaDurumlari BannerDurumu { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanUrunler>(entity =>
            {
                entity
                .HasMany(p => p.OneCikanUrunlerTranslate)
                .WithOne(p => p.OneCikanUrun)
                .HasForeignKey(p => p.OneCikanUrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<OneCikanUrunler>(entity =>
            {

                entity
                .HasMany(p => p.OneCikanUrunResimleri)
                .WithOne(p => p.OneCikanUrunler)
                .HasForeignKey(p => p.OneCikanUrunId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
    public class OneCikanUrunlerTranslate : BaseEntity
    {
        public string ModulAdi { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int OneCikanUrunId { get; set; }
        public virtual OneCikanUrunler OneCikanUrun { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanUrunlerTranslate>(entity =>
            {
                entity
               .Property(p => p.ModulAdi)
               .HasMaxLength(500);
            });
        }
    }

}