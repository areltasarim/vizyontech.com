using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class SiparisDurumlari : BaseEntity
    {

        //public virtual ICollection<Siparisler> Siparisler { get; set; }
        public virtual ICollection<OdemeMetodlari> OdemeMetodlari { get; set; }
        public virtual ICollection<SiparisGecmisleri> SiparisGecmisleri { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public int? Sira { get; set; }

        public virtual ICollection<SiparisDurumlariTranslate> SiparisDurumlariTranslate { get; set; }
        public virtual ICollection<Paytr> PaytrBasariliOdemeDurumu { get; set; }
        public virtual ICollection<Paytr> PaytrHataliOdemeDurumu { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SiparisDurumlari>(entity =>
            {
                entity
                .HasMany(p => p.SiparisDurumlariTranslate)
                .WithOne(p => p.SiparisDurumlari)
                .HasForeignKey(p => p.SiparisDurumId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class SiparisDurumlariTranslate : BaseEntity
    {


        [Display(Name = "Sipariş Durumu")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(250, ErrorMessage = "{0} en fazla 250 karakter olabilir")]
        public string SiparisDurumu { get; set; }


        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int SiparisDurumId { get; set; }
        public virtual SiparisDurumlari SiparisDurumlari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SiparisDurumlariTranslate>(entity =>
            {
                entity
               .Property(p => p.SiparisDurumu)
               .HasMaxLength(250);
            });
        }
    }

}