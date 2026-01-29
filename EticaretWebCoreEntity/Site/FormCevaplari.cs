using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class FormCevaplari : BaseEntity
    {

        public int SayfaId { get; set; }
        public int FormId { get; set; }
        public virtual Formlar Formlar { get; set; }
        public int FormBasvuruId { get; set; }

        public virtual FormBasvurulari FormBasvurulari { get; set; }
        public string Cevap { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Formlar>(entity =>
            {
                entity
                .HasMany(p => p.FormCevaplari)
                .WithOne(p => p.Formlar)
                .HasForeignKey(p => p.FormId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<FormBasvurulari>(entity =>
            {
                entity
                .HasMany(p => p.FormCevaplari)
                .WithOne(p => p.FormBasvurulari)
                .HasForeignKey(p => p.FormBasvuruId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }


}