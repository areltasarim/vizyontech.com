using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class FotografGaleriResimleri : BaseEntity
    {

        public string Resim { get; set; }
        public int Sira { get; set; }

        public int FotografGaleriId { get; set; }
        public virtual FotografGalerileri FotografGalerileri { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FotografGaleriResimleri>(entity =>
            {
                entity
               .Property(p => p.Resim)
               .HasMaxLength(1000);
            });
        }
    }

}