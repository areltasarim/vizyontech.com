using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity
{
    public class SiparisUrunSecenekleri : BaseEntity
    {
        public int SiparisId { get; set; }
        public virtual Siparisler Siparisler { get; set; }

        public int? UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        public int? UrunSecenekId { get; set; }
        public virtual UrunSecenekleri UrunSecenekleri { get; set; }

        public int? UrunSecenekDegerId { get; set; }
        public virtual UrunSecenekDegerleri UrunSecenekDegerleri { get; set; }

        public string SecenekAdi { get; set; }
        public string SecenekDegeri { get; set; }
        public UrunSecenekTipleri SecenekTipi { get; set; }
        public override void Build(ModelBuilder builder)
        {

            builder.Entity<SiparisUrunleri>(entity =>
            {
                entity
                .Property(p => p.Fiyat)
                .HasPrecision(18, 4);

                entity
                .Property(p => p.Kdv)
                .HasPrecision(18, 4);

                entity
               .Property(p => p.Toplam)
               .HasPrecision(18, 4);

            });
            builder.Entity<Siparisler>(entity =>
            {
                entity
                .HasMany(p => p.SiparisUrunSecenekleri)
                .WithOne(p => p.Siparisler)
                .HasForeignKey(p => p.SiparisId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Urunler>(entity =>
            {
                entity
                .HasMany(p => p.SiparisUrunSecenekleri)
                .WithOne(p => p.Urunler)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<UrunSecenekleri>(entity =>
            {
                entity
                .HasMany(p => p.SiparisUrunSecenekleri)
                .WithOne(p => p.UrunSecenekleri)
                .HasForeignKey(p => p.UrunSecenekId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<UrunSecenekDegerleri>(entity =>
            {
                entity
                .HasMany(p => p.SiparisUrunSecenekleri)
                .WithOne(p => p.UrunSecenekDegerleri)
                .HasForeignKey(p => p.UrunSecenekDegerId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
