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
    public class UrunSecenekleri : BaseEntity
    {

        public UrunSecenekTipleri SecenekTipi { get; set; }
        public int Sira { get; set; }
        public virtual ICollection<UrunSecenekleriTranslate> UrunSecenekleriTranslate { get; set; } = new List<UrunSecenekleriTranslate>();
        public virtual ICollection<UrunSecenekDegerleri> UrunSecenekDegerleri { get; set; }
        public virtual ICollection<UrunToUrunSecenek> UrunToUrunSecenek { get; set; }
        public virtual ICollection<UrunToUrunSecenekToUrunDeger> UrunToUrunSecenekToUrunDeger { get; set; }
        public virtual ICollection<SiparisUrunSecenekleri> SiparisUrunSecenekleri { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunSecenekleri>(entity =>
            {
                entity
                .HasMany(p => p.UrunSecenekleriTranslate)
                .WithOne(p => p.UrunSecenekleri)
                .HasForeignKey(p => p.UrunSecenekId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class UrunSecenekleriTranslate : BaseEntity
    {
        public string SecenekAdi { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int UrunSecenekId { get; set; }
        public virtual UrunSecenekleri UrunSecenekleri { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunSecenekleriTranslate>(entity =>
            {
                entity
                .Property(p => p.SecenekAdi)
                .HasMaxLength(255);
            });
        }
    }
}