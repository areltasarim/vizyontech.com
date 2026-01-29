using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity
{
    public class ParaBirimleri : BaseEntity
    {
        [Display(Name = "Para Birim Adı")]
        [Required(ErrorMessage = "{0} alani bos birakilamaz..!")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "{0} en fazla 50 karakter olabilir")]
        public string ParaBirimAdi { get; set; }

        [Display(Name = "Kodu")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public string Kodu { get; set; }
        public int DilKoduId { get; set; }
        public virtual DilKodlari DilKodu { get; set; }
        public int? Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public virtual ICollection<Paytr> Paytr { get; set; }
        public virtual ICollection<Kur> Kur { get; set; }
        public virtual ICollection<Siparisler> Siparisler { get; set; }
        public virtual ICollection<SiteAyarlari> SiteAyarlari { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<ParaBirimleri>(entity =>
            {
                entity
                .HasMany(p => p.Kur)
                .WithOne(p => p.ParaBirimi)
                .HasForeignKey(p => p.ParaBirimId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<ParaBirimleri>(entity =>
            {
                entity
                .HasMany(p => p.SiteAyarlari)
                .WithOne(p => p.AktifParaBirimi)
                .HasForeignKey(p => p.ParaBirimId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}
