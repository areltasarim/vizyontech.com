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
    public class BannerResim : BaseEntity
    {

        public virtual ICollection<BannerResimTranslate> BannerResimTranslate { get; set; }

        public int BannerId { get; set; }
        public virtual Banner Banner { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<BannerResim>(entity =>
            {
                entity
                .HasMany(p => p.BannerResimTranslate)
                .WithOne(p => p.BannerResim)
                .HasForeignKey(p => p.BannerResimId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Banner>(entity =>
            {
                entity
                .HasMany(p => p.BannerResim)
                .WithOne(p => p.Banner)
                .HasForeignKey(p => p.BannerId)
                .OnDelete(DeleteBehavior.Cascade);

            });

        }

    }

    public class BannerResimTranslate : BaseEntity
    {

        public string BannerAdi { get; set; }
        public string Url { get; set; }
        public string Resim { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }
        public int Sira { get; set; }

        public int BannerResimId { get; set; }
        public virtual BannerResim BannerResim { get; set; }


        public SeoUrlTipleri SeoUrlTipi { get; set; }

        public MenuTipleri UrlTipi { get; set; }

        public int? EntityId { get; set; }

        [NotMapped]
        public IFormFile SayfaResim { get; set; }

        public override void Build(ModelBuilder builder)
        {


            builder.Entity<BannerResimTranslate>(entity =>
            {
                entity
               .Property(p => p.Url)
               .HasMaxLength(1000);
            });

        }
    }

}