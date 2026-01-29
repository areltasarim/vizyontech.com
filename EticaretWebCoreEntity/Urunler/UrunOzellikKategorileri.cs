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
    public class UrunOzellikGruplari : BaseEntity
    {

        public int Sira { get; set; }
        public virtual ICollection<UrunOzellikGruplariTranslate> UrunOzellikGruplariTranslate { get; set; } = new List<UrunOzellikGruplariTranslate>();
        public virtual ICollection<UrunOzellikleri> UrunOzellikleri { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunOzellikGruplari>(entity =>
            {

                entity
                .HasMany(p => p.UrunOzellikGruplariTranslate)
                .WithOne(p => p.UrunOzellikGruplari)
                .HasForeignKey(p => p.UrunOzellikGrupId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }

    }

    public class UrunOzellikGruplariTranslate : BaseEntity
    {

        public string GrupAdi { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int UrunOzellikGrupId { get; set; }
        public virtual UrunOzellikGruplari UrunOzellikGruplari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunOzellikGruplariTranslate>(entity =>
            {
                entity
                .Property(p => p.GrupAdi)
                .HasMaxLength(255);
            });
        }
    }
}