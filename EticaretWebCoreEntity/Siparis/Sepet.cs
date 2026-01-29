using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity
{
    public class Sepet : BaseEntity
    {
        public int? UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }
        public string CookieId { get; set; }
        public int? UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }
        public string UrunSecenek { get; set; }
        public int Adet { get; set; }

        public string KuponKodu { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        public override void Build(ModelBuilder builder)
        {
           
            

            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.Sepet)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Urunler>(entity =>
            {
                entity
                .HasMany(p => p.Sepet)
                .WithOne(p => p.Urunler)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
