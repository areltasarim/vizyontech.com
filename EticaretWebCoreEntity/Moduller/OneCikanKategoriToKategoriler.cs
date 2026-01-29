using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class OneCikanKategoriToKategoriler : BaseEntity
    {

        public int OneCikanKategoriId { get; set; }
        public virtual OneCikanKategoriler OneCikanKategori { get; set; }

        public int KategoriId { get; set; }
        public virtual Kategoriler Kategori { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanKategoriToKategoriler>(entity =>
            {
                entity
                .HasOne(p => p.OneCikanKategori)
                .WithMany(p => p.OneCikanKategoriToKategoriler)
                .HasForeignKey(p => p.OneCikanKategoriId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<OneCikanKategoriToKategoriler>(entity =>
            {
                entity
                .HasOne(p => p.Kategori)
                .WithMany(p=> p.ModulToOneCikanKategori)
                .HasForeignKey(p=> p.KategoriId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}