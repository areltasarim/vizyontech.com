using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class UrunToSlayt : BaseEntity
    {
        public int SlaytId { get; set; }
        public virtual Slaytlar Slaytlar { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunToSlayt>(entity =>
            {
                entity
                .HasOne(p => p.Slaytlar)
                .WithMany(p => p.UrunToSlayt)
                .HasForeignKey(p => p.SlaytId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<UrunToSlayt>(entity =>
            {
                entity
                .HasOne(p => p.Urunler)
                .WithMany(p=> p.UrunToSlayt)
                .HasForeignKey(p=> p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            
        }
    }



}