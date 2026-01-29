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
    public class FormBasliklari : BaseEntity
    {

        public int SayfaId { get; set; }
        public virtual Sayfalar Sayfalar { get; set; }

        public virtual ICollection<FormBasliklariTranslate> FormBasliklariTranslate { get; set; }
        public virtual ICollection<Formlar> Formlar { get; set; }
        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FormBasliklari>(entity =>
            {
              entity
             .HasMany(p => p.FormBasliklariTranslate)
             .WithOne(p => p.FormBasliklari)
             .HasForeignKey(p => p.FormBaslikId)
             .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Sayfalar>(entity =>
            {
                entity
               .HasMany(p => p.FormBasliklari)
               .WithOne(p => p.Sayfalar)
               .HasForeignKey(p => p.SayfaId)
               .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

    public class FormBasliklariTranslate : BaseEntity
    {
        public string FormBasligi { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int FormBaslikId { get; set; }
        public virtual FormBasliklari FormBasliklari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FormBasliklariTranslate>(entity =>
            {
                entity
               .Property(p => p.FormBasligi)
               .HasMaxLength(250);
            });
            builder.Entity<Formlar>(entity =>
            {
                entity
                .HasMany(p => p.FormDegerleriTranslate)
                .WithOne(p => p.Formlar)
                .HasForeignKey(p => p.FormId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }

}