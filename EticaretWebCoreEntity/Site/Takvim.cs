using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class Takvim : BaseEntity
    {

        public virtual ICollection<TakvimTranslate> TakvimTranslate { get; set; }
        public string Renk { get; set; }

        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public SayfaDurumlari Durum { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Takvim>(entity =>
            {
                entity
                .HasMany(p => p.TakvimTranslate)
                .WithOne(p => p.Takvim)
                .HasForeignKey(p => p.TakvimId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class TakvimTranslate : BaseEntity
    {

        public string Baslik { get; set; }
        public string Aciklama { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int TakvimId { get; set; }
        public virtual Takvim Takvim { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<TakvimTranslate>(entity =>
            {
                entity
               .Property(p => p.Baslik)
               .HasMaxLength(500);
            });
        }
    }


}