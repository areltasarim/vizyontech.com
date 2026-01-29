using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class UrunToTamamlayiciUrun : BaseEntity
    {

        public int UrunId { get; set; }
        public virtual Urunler Urun { get; set; }

        public int TamamlayiciUrunId { get; set; }
        public virtual Urunler TamamlayiciUrun { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunToTamamlayiciUrun>(entity =>
            {
                entity
                .HasOne(p => p.Urun)
                .WithMany(p => p.UrunToTamamlayiciUrun)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                ;
            });

            builder.Entity<UrunToTamamlayiciUrun>(entity =>
            {
                entity
                .HasOne(p => p.Urun)
                .WithMany(p => p.UrunToTamamlayiciUrun)
                .HasForeignKey(p => p.TamamlayiciUrunId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}