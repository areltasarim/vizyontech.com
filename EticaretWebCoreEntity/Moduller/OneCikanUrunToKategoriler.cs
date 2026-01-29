using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class OneCikanUrunToKategoriler : BaseEntity
    {

        public int OneCikanUrunId { get; set; }
        public virtual OneCikanUrunler OneCikanUrun { get; set; }

        public int KategoriId { get; set; }
        public virtual Kategoriler Kategori { get; set; }

        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanUrunToKategoriler>(entity =>
            {
                entity
                .HasOne(p => p.OneCikanUrun)
                .WithMany(p => p.OneCikanUrunToKategoriler)
                .HasForeignKey(p => p.OneCikanUrunId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<OneCikanUrunToKategoriler>(entity =>
            {
                entity
                .HasOne(p => p.Kategori)
                .WithMany(p=> p.ModulToOneCikanUrunKategori)
                .HasForeignKey(p=> p.KategoriId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}