using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class SayfaToOzellik : BaseEntity
    {

        public int SayfaOzellikId { get; set; }
        public virtual SayfaOzellikleri SayfaOzellikleri { get; set; }

        public int SayfaId { get; set; }
        public virtual Sayfalar Sayfalar { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public string Aciklama { get; set; }

        public string Resim { get; set; }

        [NotMapped]
        public IFormFile OzellikResim { get; set; }

        [NotMapped]
        public bool SilinmeDurum { get; set; } = false;
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaToOzellik>(entity =>
            {
                entity
                .HasOne(p => p.SayfaOzellikleri)
                .WithMany(p => p.SayfaToOzellik)
                .HasForeignKey(p => p.SayfaOzellikId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<SayfaToOzellik>(entity =>
            {
                entity
                .HasOne(p => p.Sayfalar)
                .WithMany(p => p.SayfaToOzellik)
                .HasForeignKey(p => p.SayfaId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}