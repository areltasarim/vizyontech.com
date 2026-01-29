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
    public class Kdv : BaseEntity
    {
        [Display(Name = "Kdv Adı")]
        [Required(ErrorMessage = "{0} alani bos birakilamaz..!")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "{0} en fazla 50 karakter olabilir")]
        public string KdvAdi { get; set; }

        [Display(Name = "Kdv Oranı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public decimal KdvOrani { get; set; }
        public virtual ICollection<Urunler> Urunler { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Kdv>(entity =>
            {
               

            });
           
        }
    }


}