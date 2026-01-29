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
    public class Plasiyer : BaseEntity
    {
        public int PlasiyerId { get; set; }
        public string AdSoyad { get; set; }
        public string Gsm { get; set; }
        public string Email { get; set; }
        public string Kod { get; set; }
        public string Grup { get; set; }
        public virtual ICollection<AppUser> Uyeler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Plasiyer>(entity =>
            {
        
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
