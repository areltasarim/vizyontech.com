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
    public class UrunToOzellik : BaseEntity
    {

        public int UrunOzellikId { get; set; }
        public virtual UrunOzellikleri UrunOzellikleri { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

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
            builder.Entity<UrunToOzellik>(entity =>
            {
                entity
                .HasOne(p => p.UrunOzellikleri)
                .WithMany(p => p.UrunToOzellik)
                .HasForeignKey(p => p.UrunOzellikId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<UrunToOzellik>(entity =>
            {
                entity
                .HasOne(p => p.Urunler)
                .WithMany(p => p.UrunToOzellik)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}