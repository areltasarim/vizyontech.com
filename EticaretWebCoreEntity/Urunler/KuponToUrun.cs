using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class KuponToUrun : BaseEntity
    {

        public int KuponId { get; set; }
        public virtual Kuponlar Kupon { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urun { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<KuponToUrun>(entity =>
            {
                entity
                .HasOne(p => p.Urun)
                .WithMany(p => p.KuponToUrun)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                ;
            });

            builder.Entity<KuponToUrun>(entity =>
            {
                entity
                .HasOne(p => p.Kupon)
                .WithMany(p => p.KuponToUrun)
                .HasForeignKey(p => p.KuponId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}