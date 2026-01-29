using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class OneCikanKategoriler : BaseEntity
    {
        public virtual ICollection<OneCikanKategorilerTranslate> OneCikanKategorilerTranslate { get; set; } = new List<OneCikanKategorilerTranslate>();
        public virtual ICollection<OneCikanKategoriToKategoriler> OneCikanKategoriToKategoriler { get; set; }

        public int ModulId { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanKategoriler>(entity =>
            {
                entity
                .HasMany(p => p.OneCikanKategorilerTranslate)
                .WithOne(p => p.OneCikanKategori)
                .HasForeignKey(p => p.OneCikanKategoriId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
    public class OneCikanKategorilerTranslate : BaseEntity
    {
        public string ModulAdi { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int OneCikanKategoriId { get; set; }
        public virtual OneCikanKategoriler OneCikanKategori { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanKategorilerTranslate>(entity =>
            {
                entity
               .Property(p => p.ModulAdi)
               .HasMaxLength(500);
            });
        }
    }

}