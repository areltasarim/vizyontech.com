using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{

    public class Kuponlar : BaseEntity
    {
        public string KuponAdi { get; set; }
        public string Kod { get; set; }
        public KuponOranTipi OranTipi { get; set; }
        public decimal Indirim { get; set; }

        //Kaç TL üzerinde olursa kuponu uygulasın
        public decimal? ToplamTutar { get; set; }

        public DateTime BaslangicTarihi { get; set; }

        public DateTime BitisTarihi { get; set; }

        public SayfaDurumlari Durum { get; set; }
        public virtual ICollection<KuponToUrun> KuponToUrun { get; set; }
        public virtual ICollection<KuponToSiparis> KuponToSiparis { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Kuponlar>(entity =>
            {
                entity
               .Property(p => p.Indirim)
               .HasPrecision(18, 4);

                entity
                 .Property(p => p.ToplamTutar)
                 .HasPrecision(18, 4);

                entity
               .Property(p => p.KuponAdi)
               .HasMaxLength(250);
            });
        }
    }

}