using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class VideoKategorileri : BaseEntity
    {

        public virtual ICollection<VideoKategorileriTranslate> VideoKategorileriTranslate { get; set; }
        public virtual ICollection<Videolar> Videolar { get; set; }
        public SayfaDurumlari SilmeYetkisi { get; set; }
        public SayfaDurumlari AdminSolMenu { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public string Resim { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<VideoKategorileri>(entity =>
            {
                entity
                .HasMany(p => p.VideoKategorileriTranslate)
                .WithOne(p => p.VideoKategorileri)
                .HasForeignKey(p => p.VideoKategoriId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        
        }

    }

    public class VideoKategorileriTranslate : BaseEntity
    {

        public string KategoriAdi { get; set; }
        public string KisaAciklama { get; set; }
        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int VideoKategoriId { get; set; }
        public virtual VideoKategorileri VideoKategorileri { get; set; }

        public override void Build(ModelBuilder builder)
        {

            builder.Entity<VideoKategorileriTranslate>(entity =>
            {
                entity
               .Property(p => p.KategoriAdi)
               .HasMaxLength(255);
            });

        }
    }

}