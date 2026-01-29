using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class KuponToSiparis : BaseEntity
    {

        public int KuponId { get; set; }
        public virtual Kuponlar Kupon { get; set; }

        public int SiparisId { get; set; }
        public virtual Siparisler Siparis { get; set; }

        public int? UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }

        public decimal IndirimTutari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<KuponToSiparis>(entity =>
            {
                entity
                .Property(p => p.IndirimTutari)
                .HasPrecision(18, 4);

                entity
                .HasOne(p => p.Kupon)
                .WithMany(p => p.KuponToSiparis)
                .HasForeignKey(p => p.KuponId)
                .OnDelete(DeleteBehavior.Cascade)
                ;
            });

            builder.Entity<KuponToSiparis>(entity =>
            {
                entity
                .HasOne(p => p.Siparis)
                .WithMany(p => p.KuponToSiparis)
                .HasForeignKey(p => p.SiparisId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.KuponToSiparis)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}