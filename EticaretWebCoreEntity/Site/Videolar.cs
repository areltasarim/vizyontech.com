using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Videolar : BaseEntity
    {

        public int VideoKategoriId { get; set; }
        public virtual VideoKategorileri VideoKategorileri { get; set; }
        public VideoTipleri VideoTipi { get; set; }
        public string Resim { get; set; }
        public virtual ICollection<VideolarTranslate> VideolarTranslate { get; set; }
        public SayfaDurumlari SilmeYetkisi { get; set; }
        public SayfaDurumlari AdminSolMenu { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<VideoKategorileri>(entity =>
            {
                entity
                .HasMany(p => p.Videolar)
                .WithOne(p => p.VideoKategorileri)
                .HasForeignKey(p => p.VideoKategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Videolar>(entity =>
            {
                entity
                .HasMany(p => p.VideolarTranslate)
                .WithOne(p => p.Videolar)
                .HasForeignKey(p => p.VideoId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

    public class VideolarTranslate : BaseEntity
    {

        public string VideoAdi { get; set; }
        public string VideoLinki { get; set; }
        public string KisaAciklama { get; set; }
        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int VideoId { get; set; }
        public virtual Videolar Videolar { get; set; }

        public override void Build(ModelBuilder builder)
        {

            builder.Entity<VideolarTranslate>(entity =>
            {
                entity
               .Property(p => p.VideoAdi)
               .HasMaxLength(255);
            });

        }
    }

}