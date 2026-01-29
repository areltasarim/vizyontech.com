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
    public class SayfaOzellikGruplari : BaseEntity
    {

        public int Sira { get; set; }
        public virtual ICollection<SayfaOzellikGruplariTranslate> SayfaOzellikGruplariTranslate { get; set; } = new List<SayfaOzellikGruplariTranslate>();
        public virtual ICollection<SayfaOzellikleri> SayfaOzellikleri { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaOzellikGruplari>(entity =>
            {

                entity
                .HasMany(p => p.SayfaOzellikGruplariTranslate)
                .WithOne(p => p.SayfaOzellikGruplari)
                .HasForeignKey(p => p.SayfaOzellikGrupId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }

    }

    public class SayfaOzellikGruplariTranslate : BaseEntity
    {

        public string GrupAdi { get; set; }
        public string Aciklama { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int SayfaOzellikGrupId { get; set; }
        public virtual SayfaOzellikGruplari SayfaOzellikGruplari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaOzellikGruplariTranslate>(entity =>
            {
                entity
                .Property(p => p.GrupAdi)
                .HasMaxLength(255);
            });
        }
    }
}