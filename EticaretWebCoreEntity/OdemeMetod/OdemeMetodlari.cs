using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class OdemeMetodlari : BaseEntity
    {

        [Display(Name = "Sipariş Durumu")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int SiparisDurumId { get; set; }
        public virtual SiparisDurumlari SiparisDurumlari { get; set; }
  
        public SayfaDurumlari Durum { get; set; }
        public int? Sira { get; set; }

        public virtual ICollection<OdemeMetodlariTranslate> OdemeMetodlariTranslate { get; set; }
        public virtual ICollection<Paytr> Paytr { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OdemeMetodlari>(entity =>
            {
                entity
                .HasMany(p => p.OdemeMetodlariTranslate)
                .WithOne(p => p.OdemeMetodlari)
                .HasForeignKey(p => p.OdemeMetodId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            builder.Entity<SiparisDurumlari>(entity =>
            {
                entity
                .HasMany(p => p.OdemeMetodlari)
                .WithOne(p => p.SiparisDurumlari)
                .HasForeignKey(p => p.SiparisDurumId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class OdemeMetodlariTranslate : BaseEntity
    {


        [Display(Name = "Ödeme Adı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(250, ErrorMessage = "{0} en fazla 250 karakter olabilir")]
        public string OdemeAdi { get; set; }

        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "{0} en fazla 500 karakter olabilir")]
        public string Aciklama { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int OdemeMetodId { get; set; }
        public virtual OdemeMetodlari OdemeMetodlari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<OdemeMetodlariTranslate>(entity =>
            {
                entity
               .Property(p => p.OdemeAdi)
               .HasMaxLength(250);
            });
        }
    }

}