using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class OneCikanUrunToUrunler : BaseEntity
    {

        public int OneCikanUrunId { get; set; }
        public virtual OneCikanUrunler OneCikanUrun { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urun { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanUrunToUrunler>(entity =>
            {
                entity
                .HasOne(p => p.OneCikanUrun)
                .WithMany(p => p.OneCikanUrunToUrunler)
                .HasForeignKey(p => p.OneCikanUrunId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<OneCikanUrunToUrunler>(entity =>
            {
                entity
                .HasOne(p => p.Urun)
                .WithMany(p=> p.ModulToOneCikanUrun)
                .HasForeignKey(p=> p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}