using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class UrunOzellikleri : BaseEntity
    {
        public virtual ICollection<UrunOzellikleriTranslate> UrunOzellikleriTranslate { get; set; } = new List<UrunOzellikleriTranslate>();

        public virtual ICollection<UrunToOzellik> UrunToOzellik { get; set; }
        public int UrunOzellikGrupId { get; set; }
        public virtual UrunOzellikGruplari UrunOzellikGruplari { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunOzellikGruplari>(entity =>
            {

                entity
                .HasMany(p => p.UrunOzellikleri)
                .WithOne(p => p.UrunOzellikGruplari)
                .HasForeignKey(p => p.UrunOzellikGrupId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<UrunOzellikleri>(entity =>
            {
                entity
                .HasMany(p => p.UrunOzellikleriTranslate)
                .WithOne(p => p.UrunOzellikleri)
                .HasForeignKey(p => p.UrunOzellikId)
                .OnDelete(DeleteBehavior.Cascade);

            });

        }

    }

    public class UrunOzellikleriTranslate : BaseEntity
    {

        public string OzellikAdi { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int UrunOzellikId { get; set; }
        public virtual UrunOzellikleri UrunOzellikleri { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunOzellikleriTranslate>(entity =>
            {
          
            });

        }
    }

}