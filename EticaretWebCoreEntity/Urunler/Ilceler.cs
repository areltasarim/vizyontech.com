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
    public class Ilceler : BaseEntity
    {

        [Display(Name = "İl")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int IlId { get; set; }
        public virtual Iller Iller { get; set; }

        [Display(Name = "İlçe")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public string IlceAdi { get; set; }
        public int? Sira { get; set; }
        
        public SayfaDurumlari Durum { get; set; }
        public virtual ICollection<Adresler> Adresler { get; set; }
        public virtual ICollection<AppUser> Uyeler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Iller>(entity =>
            {

                entity
                   .HasMany(p => p.Ilceler)
                   .WithOne(p => p.Iller)
                   .HasForeignKey(p => p.IlId)
                   .OnDelete(DeleteBehavior.Restrict);
            });


        }
    }


}