using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class UrunToBenzerUrun : BaseEntity
    {

        public int UrunId { get; set; }
        public virtual Urunler Urun { get; set; }

        public int BenzerUrunId { get; set; }
        public virtual Urunler BenzerUrun { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunToBenzerUrun>(entity =>
            {
                entity
                .HasOne(p => p.Urun)
                .WithMany(p => p.UrunToBenzerUrun)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                ;
            });

            builder.Entity<UrunToBenzerUrun>(entity =>
            {
                entity
                .HasOne(p => p.Urun)
                .WithMany(p => p.UrunToBenzerUrun)
                .HasForeignKey(p => p.BenzerUrunId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}