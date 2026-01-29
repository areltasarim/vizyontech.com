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
    public class UrunToUrunSecenekToUrunDeger : BaseEntity
    {

        public int Adet { get; set; }
        public decimal Fiyat { get; set; }
        public int UrunToUrunSecenekId { get; set; }
        public virtual UrunToUrunSecenek UrunToUrunSecenek { get; set; }

        public int UrunSecenekId { get; set; }
        public virtual UrunSecenekleri UrunSecenekleri { get; set; }

        public int UrunSecenekDegerId { get; set; }
        public virtual UrunSecenekDegerleri UrunSecenekDegerleri { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunToUrunSecenekToUrunDeger>(entity =>
            {
                entity
                .Property(p => p.Fiyat)
                .HasPrecision(18, 4);
            });

            builder.Entity<UrunToUrunSecenekToUrunDeger>(entity =>
            {
                entity
                .HasOne(p => p.UrunToUrunSecenek)
                .WithMany(p => p.UrunToUrunSecenekToUrunDeger)
                .HasForeignKey(p => p.UrunToUrunSecenekId)
                .OnDelete(DeleteBehavior.Cascade)
                ;
            });

            builder.Entity<UrunToUrunSecenekToUrunDeger>(entity =>
            {
                entity
                .HasOne(p => p.UrunSecenekleri)
                .WithMany(p => p.UrunToUrunSecenekToUrunDeger)
                .HasForeignKey(p => p.UrunSecenekId)
                .OnDelete(DeleteBehavior.NoAction)
                ;
            });

            builder.Entity<UrunToUrunSecenekToUrunDeger>(entity =>
            {
                entity
                .HasOne(p => p.UrunSecenekDegerleri)
                .WithMany(p => p.UrunToUrunSecenekToUrunDeger)
                .HasForeignKey(p => p.UrunSecenekDegerId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UrunToUrunSecenekToUrunDeger>(entity =>
            {
                entity
                .HasOne(p => p.Urunler)
                .WithMany(p => p.UrunToUrunSecenekToUrunDeger)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}