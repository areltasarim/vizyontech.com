using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity
{
    public class Fiyatlar : BaseEntity
    {
        public int IlId { get; set; }
        public virtual Iller Iller { get; set; }
        public DateTime Tarih { get; set; }
        public decimal Fiyat { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Fiyatlar>(entity =>
            {
                entity
               .Property(p => p.Fiyat)
               .HasPrecision(18, 4);
            });

            builder.Entity<Iller>(entity =>
            {
                entity
                .HasMany(p => p.Fiyatlar)
                .WithOne(p => p.Iller)
                .HasForeignKey(p => p.IlId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}