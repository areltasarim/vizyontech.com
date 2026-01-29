using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Slaytlar : BaseEntity
    {
        public string ArkaplanResim { get; set; }
        public string Resim1 { get; set; }
        public string Resim2 { get; set; }
        public string Resim3 { get; set; }
        public string Resim4 { get; set; }
        public string Resim5 { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public int Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public virtual ICollection<SlaytlarTranslate> SlaytlarTranslate { get; set; }
        public virtual ICollection<UrunToSlayt> UrunToSlayt { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Slaytlar>(entity =>
            {
                entity
                .HasMany(p => p.SlaytlarTranslate)
                .WithOne(p => p.Slaytlar)
                .HasForeignKey(p => p.SlaytId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class SlaytlarTranslate : BaseEntity
    {

        public string SlaytBaslik { get; set; }
        public string SlaytBaslik2 { get; set; }
        public string SlaytBaslik3 { get; set; }
        public string SlaytBaslik4 { get; set; }
        public string SlaytBaslik5 { get; set; }

        [DataType(DataType.MultilineText)]
        public string Aciklama { get; set; }
        public string ButonAdi { get; set; }
        public string Url { get; set; }

        public string ButonAdi2 { get; set; }
        public string Url2 { get; set; }
        public string Resim { get; set; }
        public string Video { get; set; }
        public string YoutubeVideo { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int SlaytId { get; set; }
        public virtual Slaytlar Slaytlar { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SlaytlarTranslate>(entity =>
            {
                entity
               .Property(p => p.SlaytBaslik)
               .HasMaxLength(500);
            });
        }
    }

}