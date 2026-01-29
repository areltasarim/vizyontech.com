using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class SeoUrl : BaseEntity
    {
        public int? DilId { get; set; }
        public virtual Diller Diller { get; set; }
        public string Url { get; set; }
        public SeoUrlTipleri EntityName { get; set; }
        public SeoTipleri SeoTipi { get; set; }
        public int EntityId { get; set; }

        //public virtual ICollection<Menuler> Menuler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SeoUrl>(entity =>
            {
                entity
               .Property(p => p.EntityName)
               .HasMaxLength(255);
            });
        }
    }

}