using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class SayfaOzellikleri : BaseEntity
    {
        public virtual ICollection<SayfaOzellikleriTranslate> SayfaOzellikleriTranslate { get; set; } = new List<SayfaOzellikleriTranslate>();

        public virtual ICollection<SayfaToOzellik> SayfaToOzellik { get; set; }
        public int SayfaOzellikGrupId { get; set; }
        public virtual SayfaOzellikGruplari SayfaOzellikGruplari { get; set; }


        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaOzellikGruplari>(entity =>
            {

                entity
                .HasMany(p => p.SayfaOzellikleri)
                .WithOne(p => p.SayfaOzellikGruplari)
                .HasForeignKey(p => p.SayfaOzellikGrupId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<SayfaOzellikleri>(entity =>
            {
                entity
                .HasMany(p => p.SayfaOzellikleriTranslate)
                .WithOne(p => p.SayfaOzellikleri)
                .HasForeignKey(p => p.SayfaOzellikId)
                .OnDelete(DeleteBehavior.Cascade);

            });

   

        }

    }

    public class SayfaOzellikleriTranslate : BaseEntity
    {

        public string OzellikAdi { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int SayfaOzellikId { get; set; }
        public virtual SayfaOzellikleri SayfaOzellikleri { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaOzellikleriTranslate>(entity =>
            {
          
            });

        }
    }

}