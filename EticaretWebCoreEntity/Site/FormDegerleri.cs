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
    public class FormDegerleri : BaseEntity
    {

        public virtual ICollection<FormDegerleriTranslate> FormDegerleriTranslate { get; set; }

        public int FormId { get; set; }
        public virtual Formlar Formlar { get; set; }

        public int? Sira { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FormDegerleri>(entity =>
            {
                entity
               .HasMany(p => p.FormDegerleriTranslate)
               .WithOne(p => p.FormDegerleri)
               .HasForeignKey(p => p.FormDegerId)
               .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Formlar>(entity =>
            {
                entity
                .HasMany(p => p.FormDegerleri)
                .WithOne(p => p.Formlar)
                .HasForeignKey(p => p.FormId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }

    }

    public class FormDegerleriTranslate : BaseEntity
    {
        public string DegerAdi { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int FormDegerId { get; set; }
        public virtual FormDegerleri FormDegerleri { get; set; }


        public int FormId { get; set; }
        public virtual Formlar Formlar { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FormDegerleriTranslate>(entity =>
            {
                entity
               .Property(p => p.DegerAdi)
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