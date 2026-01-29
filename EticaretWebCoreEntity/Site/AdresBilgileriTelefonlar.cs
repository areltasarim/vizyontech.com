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
    public class AdresBilgileriTelefonlar : BaseEntity
    {

        public virtual ICollection<AdresBilgileriTelefonlarTranslate> AdresBilgileriTelefonlarTranslate { get; set; }

        public int AdresBilgiId { get; set; }
        public virtual AdresBilgileri AdresBilgileri { get; set; }

        public SayfaDurumlari Durum { get; set; }

        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<AdresBilgileriTelefonlar>(entity =>
            {
                entity
                .HasMany(p => p.AdresBilgileriTelefonlarTranslate)
                .WithOne(p => p.AdresBilgileriTelefonlar)
                .HasForeignKey(p => p.AdresBilgileriTelefonId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<AdresBilgileri>(entity =>
            {
                entity
                .HasMany(p => p.AdresBilgileriTelefonlar)
                .WithOne(p => p.AdresBilgileri)
                .HasForeignKey(p => p.AdresBilgiId)
                .OnDelete(DeleteBehavior.Restrict);

            });

        }

    }

    public class AdresBilgileriTelefonlarTranslate : BaseEntity
    {

        public string Telefon { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int AdresBilgileriTelefonId { get; set; }
        public virtual AdresBilgileriTelefonlar AdresBilgileriTelefonlar { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<AdresBilgileriTelefonlarTranslate>(entity =>
            {
                entity
               .Property(p => p.Telefon)
               .HasMaxLength(20);
            });

        }
    }

}