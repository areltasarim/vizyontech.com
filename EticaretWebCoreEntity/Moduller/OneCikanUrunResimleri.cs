using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class OneCikanUrunResimleri : BaseEntity
    {
        public string Resim { get; set; }
        public int Sira { get; set; }

        public int OneCikanUrunId { get; set; }
        public virtual OneCikanUrunler OneCikanUrunler { get; set; }

        public int? DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OneCikanUrunResimleri>(entity =>
            {
                entity
               .Property(p => p.Resim)
               .HasMaxLength(1000);
            });
        }
    }


}