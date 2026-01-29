using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class SayfaToSayfalar : BaseEntity
    {

        public int SayfaId { get; set; }
        public virtual Sayfalar Sayfa { get; set; }

        public int SayfalarId { get; set; }
        public virtual Sayfalar Sayfalar { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaToSayfalar>(entity =>
            {
                entity
                .HasOne(p => p.Sayfalar)
                .WithMany(p => p.SayfaToSayfalar)
                .HasForeignKey(p => p.SayfaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                ;
            });

            builder.Entity<SayfaToSayfalar>(entity =>
            {
                entity
                .HasOne(p => p.Sayfalar)
                .WithMany(p => p.SayfaToSayfalar)
                .HasForeignKey(p => p.SayfalarId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

        }
    }

}