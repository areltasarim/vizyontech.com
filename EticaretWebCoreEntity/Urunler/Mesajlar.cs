using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Mesajlar : BaseEntity
    {
        public int MesajKonuId { get; set; }
        public virtual MesajKonulari MesajKonulari { get; set; }

        [Display(Name = "Üye")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; } 

        public int GonderilenUyeId { get; set; }

        [Display(Name = "Mesaj")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(1000, ErrorMessage = "{0} en fazla 1000 karakter olabilir")]
        public string Mesaj { get; set; }
        public DateTime MesajTarihi { get; set; }
        public MesajOkunmaDurumlari OkunmaDurumu { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.Mesajlar)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<MesajKonulari>(entity =>
            {
                entity
                .HasMany(p => p.Mesajlar)
                .WithOne(p => p.MesajKonulari)
                .HasForeignKey(p => p.MesajKonuId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}