using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Logs : BaseEntity
    {
        public DateTime Tarih { get; set; }
        public string Mesaj { get; set; }
        public LogTipleri LogTipi { get; set; }
        public int? UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Logs>(entity =>
            {
               
            });


            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.Logs)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

}