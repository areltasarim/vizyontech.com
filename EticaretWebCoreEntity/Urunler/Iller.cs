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
    public class Iller : BaseEntity
    {
        [Display(Name = "Ülke")]
        [Required(ErrorMessage = "{0} alani bos birakilamaz..!")]
        public int UlkeId { get; set; }
        public virtual Ulkeler Ulkeler { get; set; }

        [Display(Name = "İl Adı")]
        [Required(ErrorMessage = "{0} alani bos birakilamaz..!")]
        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage = "{0} en fazla 100 karakter olabilir")]
        public string IlAdi { get; set; }
        public string Plaka { get; set; }
        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public virtual ICollection<Ilceler> Ilceler { get; set; }
        public virtual ICollection<Fiyatlar> Fiyatlar { get; set; }
        public virtual ICollection<Bayiler> Bayiler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Ulkeler>(entity =>
            {
                entity
                   .HasMany(p => p.Iller)
                   .WithOne(p => p.Ulkeler)
                   .HasForeignKey(p => p.UlkeId)
                   .OnDelete(DeleteBehavior.Restrict);
            });


        }
    }


}