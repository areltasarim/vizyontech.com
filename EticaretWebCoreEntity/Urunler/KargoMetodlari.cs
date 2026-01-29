using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class KargoMetodlari : BaseEntity
    {

        public decimal Fiyat { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public int? Sira { get; set; }

        public virtual ICollection<KargoMetodlariTranslate> KargoMetodlariTranslate { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<KargoMetodlari>(entity =>
            {
                entity
               .Property(p => p.Fiyat)
               .HasPrecision(18, 4);


                entity
                .HasMany(p => p.KargoMetodlariTranslate)
                .WithOne(p => p.KargoMetodlari)
                .HasForeignKey(p => p.KargoMetodId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class KargoMetodlariTranslate : BaseEntity
    {


        [Display(Name = "Kargo Adı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(250, ErrorMessage = "{0} en fazla 250 karakter olabilir")]
        public string KargoAdi { get; set; }

        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "{0} en fazla 250 karakter olabilir")]
        public string Aciklama { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int KargoMetodId { get; set; }
        public virtual KargoMetodlari KargoMetodlari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<KargoMetodlariTranslate>(entity =>
            {
 
                entity
               .Property(p => p.KargoAdi)
               .HasMaxLength(250);
            });
        }
    }

}