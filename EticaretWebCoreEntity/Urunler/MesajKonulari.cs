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
    public class MesajKonulari : BaseEntity
    {
        [Display(Name = "Üye")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }

        public int GonderilenUyeId { get; set; }
        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        [Display(Name = "Konu")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(250, ErrorMessage = "{0} en fazla 250 karakter olabilir")]
        public string Konu { get; set; }
        public virtual ICollection<Mesajlar> Mesajlar { get; set;}
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<AppUser>(entity =>
            {
                entity
                .HasMany(p => p.MesajKonulari)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            builder.Entity<Urunler>(entity =>
            {
                entity
                .HasMany(p => p.MesajKonulari)
                .WithOne(p => p.Urunler)
                .HasForeignKey(p => p.UrunId)
                .OnDelete(DeleteBehavior.Restrict);
            });

        }

        [NotMapped]
        public bool OkunmaDurum { get; set; }
    }
}