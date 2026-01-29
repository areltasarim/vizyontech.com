using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class UrunToUrunSecenek : BaseEntity
    {
        public int UrunSecenekId { get; set; }
        public virtual UrunSecenekleri UrunSecenekleri { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        public virtual ICollection<UrunToUrunSecenekToUrunDeger> UrunToUrunSecenekToUrunDeger { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunToUrunSecenek>(entity =>
            {
                entity
                .HasOne(p => p.UrunSecenekleri)
                .WithMany(p => p.UrunToUrunSecenek)
                .HasForeignKey(p => p.UrunSecenekId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<UrunToUrunSecenek>(entity =>
            {
                entity
                .HasOne(p => p.Urunler)
                .WithMany(p => p.UrunToUrunSecenek)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}