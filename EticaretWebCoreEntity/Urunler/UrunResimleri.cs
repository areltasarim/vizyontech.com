using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class UrunResimleri : BaseEntity
    {

        public string ResimAdi { get; set; }
        public string Resim { get; set; }
        public int Sira { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }
        public UrunResimKategorileri UrunResimKategori { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<UrunResimleri>(entity =>
            {
                entity
               .Property(p => p.Resim)
               .HasMaxLength(1000);
            });
        }
    }

}