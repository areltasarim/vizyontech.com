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
    public class Formlar : BaseEntity
    {
        public int FormBaslikId { get; set; }
        public virtual FormBasliklari FormBasliklari { get; set; }

        public virtual ICollection<FormlarTranslate> FormlarTranslate { get; set; }
        public virtual ICollection<FormDegerleri> FormDegerleri { get; set; }
        public virtual ICollection<FormDegerleriTranslate> FormDegerleriTranslate { get; set; }
        public virtual ICollection<FormCevaplari> FormCevaplari { get; set; }
        public FormTurleri FormTuru { get; set; }

        public FormDurumlari Zorunlumu { get; set; }
        public FormGenislikleri Genislik { get; set; }
        public bool TexboxTipi { get; set; }
        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Formlar>(entity =>
            {
              entity
             .HasMany(p => p.FormlarTranslate)
             .WithOne(p => p.Formlar)
             .HasForeignKey(p => p.FormId)
             .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<FormBasliklari>(entity =>
            {
                entity
               .HasMany(p => p.Formlar)
               .WithOne(p => p.FormBasliklari)
               .HasForeignKey(p => p.FormBaslikId)
               .OnDelete(DeleteBehavior.Cascade);
            });

           

        }

    }

    public class FormlarTranslate : BaseEntity
    {
        public string FormAdi { get; set; }
        public string PlaceHolder { get; set; }
        public string HataMesaji { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int FormId { get; set; }
        public virtual Formlar Formlar { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FormlarTranslate>(entity =>
            {
                entity
               .Property(p => p.FormAdi)
               .HasMaxLength(250);
            });

        }
    }

}