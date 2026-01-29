using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity
{
    public class SiparisUrunleri : BaseEntity
    {
        public int SiparisId { get; set; }
        public virtual Siparisler Siparisler { get; set; }

        public int? UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        public string UrunAdi { get; set; }
        public string Marka { get; set; }
        public string UrunKodu { get; set; }
        public string Not { get; set; }
        public int Adet { get; set; }
        public decimal Fiyat { get; set; }
        public decimal Kdv { get; set; }
        public decimal Toplam { get; set; }

        public override void Build(ModelBuilder builder)
        {

            builder.Entity<SiparisUrunleri>(entity =>
            {
                entity
                .Property(p => p.Fiyat)
                .HasPrecision(18, 4);

                entity
                .Property(p => p.Kdv)
                .HasPrecision(18, 4);

                entity
               .Property(p => p.Toplam)
               .HasPrecision(18, 4);

            });
            builder.Entity<Siparisler>(entity =>
            {
                entity
                .HasMany(p => p.SiparisUrunleri)
                .WithOne(p => p.Siparisler)
                .HasForeignKey(p => p.SiparisId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Urunler>(entity =>
            {
                entity
                .HasMany(p => p.SiparisUrunleri)
                .WithOne(p => p.Urunler)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
