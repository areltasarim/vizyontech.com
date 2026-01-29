using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity
{
    public class Bayiler : BaseEntity
    {
        public int IlId { get; set; }
        public virtual Iller Iller { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Bayi Adı Boş Bırakılamaz.")]
        [MaxLength(400, ErrorMessage = "Bayi Adı 400 Karakterden Fazla Olamaz.")]
        public string BayiAdi { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Harita { get; set; }
        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Bayiler>(entity =>
            {
                entity
               .Property(p => p.BayiAdi)
               .HasMaxLength(400);
            });

            builder.Entity<Iller>(entity =>
            {

                entity
                .HasMany(p => p.Bayiler)
                .WithOne(p => p.Iller)
                .HasForeignKey(p => p.IlId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}