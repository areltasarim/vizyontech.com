using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class Kur : BaseEntity
    {
        public decimal TLKur { get; set; }
        public int ParaBirimId { get; set; }
        public virtual ParaBirimleri ParaBirimi { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Kur>(entity =>
            {
                entity
               .Property(p => p.TLKur)
                .HasPrecision(18, 4);
            });
        }
    }

}