using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using EticaretWebCoreEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Yorumlar : BaseEntity
    {

        [Display(Name = "Üye")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }


        public int SayfaId { get; set; }
        public virtual Sayfalar Sayfalar { get; set; }

        public int? UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string Sehir { get; set; }
        public string Resim { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string Yorum { get; set; }
        public DateTime YorumTarihi { get; set; }
        public SayfaDurumlari YorumDurumu { get; set; }
        public Yildizlar Yildiz { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.Yorumlar)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Sayfalar>(entity =>
            {
                entity
                .HasMany(p => p.Yorumlar)
                .WithOne(p => p.Sayfalar)
                .HasForeignKey(p => p.SayfaId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Urunler>(entity =>
            {
                entity
                .HasMany(p => p.Yorumlar)
                .WithOne(p => p.Urunler)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Restrict);
            });

        }

    }

}