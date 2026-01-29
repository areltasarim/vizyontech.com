using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class SayfaResimleri : BaseEntity
    {

        public string ResimAdi { get; set; }
        public string Resim { get; set; }
        public int Sira { get; set; }

        public int SayfaId { get; set; }
        public virtual Sayfalar Sayfalar { get; set; }

        public int? DilId { get; set; }
        public virtual Diller Diller { get; set; }
        public SayfaResimKategorileri SayfaResimKategori { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaResimleri>(entity =>
            {
                entity
               .Property(p => p.Resim)
               .HasMaxLength(1000);
            });
        }
    }

}