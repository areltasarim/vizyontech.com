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
    public class SiparisGecmisleri : BaseEntity
    {
        public int SiparisId { get; set; }
        public virtual Siparisler Siparisler { get; set; }

        public int SiparisDurumId { get; set; }
        public virtual SiparisDurumlari SiparisDurumlari { get; set; }

        public string Aciklama { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        public override void Build(ModelBuilder builder)
        {

            builder.Entity<Siparisler>(entity =>
            {
                entity
                .HasMany(p => p.SiparisGecmisleri)
                .WithOne(p => p.Siparisler)
                .HasForeignKey(p => p.SiparisId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SiparisDurumlari>(entity =>
            {
                entity
                .HasMany(p => p.SiparisGecmisleri)
                .WithOne(p => p.SiparisDurumlari)
                .HasForeignKey(p => p.SiparisDurumId)
                .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
