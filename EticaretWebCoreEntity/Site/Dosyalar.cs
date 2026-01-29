using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Dosyalar : BaseEntity
    {
        public virtual ICollection<DosyalarTranslate> DosyalarTranslate { get; set; }
        public virtual ICollection<DosyaGaleri> DosyaGaleri { get; set; }
        public virtual ICollection<Urunler> DosyaDataSheet { get; set; }
        public int DosyaKategoriId { get; set; }
        public virtual DosyaKategorileri DosyaKategorileri { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Dosyalar>(entity =>
            {

                entity
                .HasMany(p => p.DosyalarTranslate)
                .WithOne(p => p.Dosyalar)
                .HasForeignKey(p => p.DosyaId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<DosyaKategorileri>(entity =>
            {
                entity
                .HasMany(p => p.Dosyalar)
                .WithOne(p => p.DosyaKategorileri)
                .HasForeignKey(p => p.DosyaKategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }

       
    }
    public class DosyalarTranslate : BaseEntity
    {

        public string DosyaAdi { get; set; }
        public string Dosya { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int DosyaId { get; set; }
        public virtual Dosyalar Dosyalar { get; set; }


        public override void Build(ModelBuilder builder)
        {


            builder.Entity<DosyalarTranslate>(entity =>
            {
                entity
                .Property(p => p.Dosya)
                .HasMaxLength(500);
            });
        }
    }
}