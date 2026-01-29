using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class Ekipler : BaseEntity
    {
        public virtual ICollection<EkiplerTranslate> EkiplerTranslate { get; set; }
        public string AdSoyad { get; set; }
        public string Resim { get; set; }
        public string Logo { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Pinterest { get; set; }
        public string GooglePlus { get; set; }
        public string Youtube { get; set; }
        public string Whatsapp { get; set; }
        public EkipKategorileri Kategori { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Ekipler>(entity =>
            {
                entity
                .HasMany(p => p.EkiplerTranslate)
                .WithOne(p => p.Ekipler)
                .HasForeignKey(p => p.EkipId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class EkiplerTranslate : BaseEntity
    {

        public string Aciklama { get; set; }
        public string Gorev { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int EkipId { get; set; }
        public virtual Ekipler Ekipler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<EkiplerTranslate>(entity =>
            {
           
            });
        }
    }


}