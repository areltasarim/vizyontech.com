using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{

    public class KategoriBanner : BaseEntity
    {

        public int KategoriId { get; set; }
        public virtual Kategoriler Kategori { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public string Url { get; set; }

        public string Resim { get; set; }
        public int? Sira { get; set; }

        [NotMapped]
        public IFormFile SayfaResim { get; set; }

        [NotMapped]
        public bool SilinmeDurum { get; set; } = false;
        public override void Build(ModelBuilder builder)
        {

            builder.Entity<Kategoriler>(entity =>
            {

                entity
                .HasMany(p => p.KategoriBanner)
                .WithOne(p => p.Kategori)
                .HasForeignKey(p => p.KategoriId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}