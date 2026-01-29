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
    public class CariOdeme : BaseEntity
    {
        public DateTime OdemeTarihi { get; set; }
        public int UyeId { get; set; }
        public virtual AppUser Uye { get; set; }
        public decimal OdenenTutar { get; set; }
        public string ZiraatPaySiparisId { get; set; }
        public ZiraatPayOdemeDurumu OdemeDurumu { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<CariOdeme>(entity =>
            {
                entity
                .Property(p => p.OdenenTutar)
                .HasPrecision(18, 4);
            });

            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.CariOdeme)
                .WithOne(p => p.Uye)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }


}
