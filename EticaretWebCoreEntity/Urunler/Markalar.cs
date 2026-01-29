using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Markalar : BaseEntity
    {
        public string MarkaAdi { get; set; }
        public string Url { get; set; }
        public string Resim { get; set; }
        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public virtual ICollection<Urunler> Urunler { get; set; }
        [NotMapped]
        public string EncrypedId { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Markalar>(entity =>
            {
                entity
               .Property(p => p.MarkaAdi)
               .HasMaxLength(250);
            });
        }
    }

}