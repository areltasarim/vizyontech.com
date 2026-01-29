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
    public class Banner : BaseEntity
    {

        public virtual ICollection<BannerTranslate> BannerTranslate { get; set; } = new List<BannerTranslate>();
        public virtual ICollection<BannerResim> BannerResim { get; set; }
        public ColumnSayisi ColumnDesktop { get; set; }
        public ColumnSayisi ColumnMobil { get; set; }
        public int Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Banner>(entity =>
            {
                entity
                .HasMany(p => p.BannerTranslate)
                .WithOne(p => p.Banner)
                .HasForeignKey(p => p.BannerId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class BannerTranslate : BaseEntity
    {
        public string BannerAdi { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }
        public int BannerId { get; set; }
        public virtual Banner Banner { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<BannerTranslate>(entity =>
            {
   
            });
        }
    }

}