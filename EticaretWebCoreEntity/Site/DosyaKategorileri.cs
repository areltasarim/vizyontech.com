using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class DosyaKategorileri : BaseEntity
    {
        public virtual ICollection<DosyaKategorileriTranslate> DosyaKategorileriTranslate { get; set; }
        public virtual ICollection<Dosyalar> Dosyalar { get; set; }

        public SayfaDurumlari Durum { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<DosyaKategorileri>(entity =>
            {

                entity
                .HasMany(p => p.DosyaKategorileriTranslate)
                .WithOne(p => p.DosyaKategorileri)
                .HasForeignKey(p => p.DosyaKategoriId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }


    }
    public class DosyaKategorileriTranslate : BaseEntity
    {

        public string KategoriAdi { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int DosyaKategoriId { get; set; }
        public virtual DosyaKategorileri DosyaKategorileri { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<DosyaKategorileriTranslate>(entity =>
            {
                entity
                .Property(p => p.KategoriAdi)
                .HasMaxLength(500);
            });
        }
    }
}