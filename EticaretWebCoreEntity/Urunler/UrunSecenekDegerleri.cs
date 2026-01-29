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
    public class UrunSecenekDegerleri : BaseEntity
    {
        public int UrunSecenekId { get; set; }
        public virtual UrunSecenekleri UrunSecenekleri { get; set; }
        public int Sira { get; set; }
        public virtual ICollection<UrunSecenekDegerleriTranslate> UrunSecenekDegerleriTranslate { get; set; }
        public virtual ICollection<UrunToUrunSecenekToUrunDeger> UrunToUrunSecenekToUrunDeger { get; set; }
        public virtual ICollection<SiparisUrunSecenekleri> SiparisUrunSecenekleri { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunSecenekleri>(entity =>
            {
                entity
                .HasMany(p => p.UrunSecenekDegerleri)
                .WithOne(p => p.UrunSecenekleri)
                .HasForeignKey(p => p.UrunSecenekId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<UrunSecenekDegerleri>(entity =>
            {
                entity
                .HasMany(p => p.UrunSecenekDegerleriTranslate)
                .WithOne(p => p.UrunSecenekDegerleri)
                .HasForeignKey(p => p.UrunSecenekDegerId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class UrunSecenekDegerleriTranslate : BaseEntity
    {

        public string DegerAdi { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }
        public int UrunSecenekDegerId { get; set; }
        public virtual UrunSecenekDegerleri UrunSecenekDegerleri { get; set; }

        [NotMapped]
        public bool SilinmeDurum { get; set; } = false;
        [NotMapped]
        public int Sira { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunSecenekDegerleriTranslate>(entity =>
            {
                entity
                .Property(p => p.DegerAdi)
                .HasMaxLength(255);
            });
        }
    }
}