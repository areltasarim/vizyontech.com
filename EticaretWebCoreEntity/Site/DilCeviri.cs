using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class DilCeviri : BaseEntity
    {
        public virtual ICollection<DilCeviriTranslate> DilCeviriTranslate { get; set; } = new List<DilCeviriTranslate>();

        public string Anahtar { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<DilCeviri>(entity =>
            {
               entity
              .HasMany(p => p.DilCeviriTranslate)
              .WithOne(p => p.DilCeviri)
              .HasForeignKey(p => p.DilCeviriId)
              .OnDelete(DeleteBehavior.Cascade);
            });
        }

      
    }
    public class DilCeviriTranslate : BaseEntity
    {

        public string Deger { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int DilCeviriId { get; set; }
        public virtual DilCeviri DilCeviri { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<DilCeviriTranslate>(entity =>
            {
                entity
               .Property(p => p.Deger)
               .HasMaxLength(1000);
            });
        }
    }

}