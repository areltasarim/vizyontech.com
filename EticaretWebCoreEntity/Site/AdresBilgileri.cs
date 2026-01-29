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
    public class AdresBilgileri : BaseEntity
    {

        public virtual ICollection<AdresBilgileriTranslate> AdresBilgileriTranslate { get; set; }
        public virtual ICollection<AdresBilgileriTelefonlar> AdresBilgileriTelefonlar { get; set; }

        public string Resim { get; set; }
        public int SiteAyarId { get; set; }
        public virtual SiteAyarlari SiteAyarlari { get; set; }

        public SayfaDurumlari Durum { get; set; }

        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<AdresBilgileri>(entity =>
            {
                entity
                .HasMany(p => p.AdresBilgileriTranslate)
                .WithOne(p => p.AdresBilgileri)
                .HasForeignKey(p => p.AdresBilgiId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<SiteAyarlari>(entity =>
            {
                entity
                .HasMany(p => p.AdresBilgileri)
                .WithOne(p => p.SiteAyarlari)
                .HasForeignKey(p => p.SiteAyarId)
                .OnDelete(DeleteBehavior.Restrict);

            });

        }

    }

    public class AdresBilgileriTranslate : BaseEntity
    {

        public string AdresBasligi { get; set; }
        public string Telefon { get; set; }
        public string Faks { get; set; }
        public string Gsm { get; set; }
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Adres { get; set; }

        [DataType(DataType.MultilineText)]
        public string Harita { get; set; }

        [DataType(DataType.MultilineText)]
        public string HaritaLink { get; set; }

        [DataType(DataType.MultilineText)]
        public string CalismaSaatlari { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int AdresBilgiId { get; set; }
        public virtual AdresBilgileri AdresBilgileri { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<AdresBilgileriTranslate>(entity =>
            {
                entity
               .Property(p => p.Telefon)
               .HasMaxLength(20);
            });

        }
    }

}