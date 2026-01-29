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
    public class Ulkeler : BaseEntity
    {
        [Display(Name = "Ülke Adı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(250, ErrorMessage = "{0} en fazla 250 karakter olabilir")]
        public string UlkeAdi { get; set; }
        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public virtual ICollection<Iller> Iller { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Ulkeler>(entity =>
            {
                

            });


        }
    }


}