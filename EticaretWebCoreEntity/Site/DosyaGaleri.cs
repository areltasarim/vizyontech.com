using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class DosyaGaleri : BaseEntity
    {
        
        public string DosyaAdi { get; set; }
        public string Dosya { get; set; }
        public int Sira { get; set; }

        public int DosyaId { get; set; }
        public virtual Dosyalar Dosyalar { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<DosyaGaleri>(entity =>
            {
                entity
               .Property(p => p.Dosya)
               .HasMaxLength(1000);
            });

            builder.Entity<Dosyalar>(entity =>
            {

                entity
                .HasMany(p => p.DosyaGaleri)
                .WithOne(p => p.Dosyalar)
                .HasForeignKey(p => p.DosyaId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

}