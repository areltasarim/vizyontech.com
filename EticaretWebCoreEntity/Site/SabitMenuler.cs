using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class SabitMenuler : BaseEntity
    {

        public virtual ICollection<SabitMenulerTranslate> SabitMenulerTranslate { get; set; }

        //public virtual ICollection<Menuler> Menuler { get; set; }

        public virtual SabitSayfaTipleri SayfaTipi { get; set; }
        public string BreadcrumbResim { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public int Sira { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SabitMenuler>(entity =>
            {
                entity
                .HasMany(p => p.SabitMenulerTranslate)
                .WithOne(p => p.SabitMenuler)
                .HasForeignKey(p => p.SabitMenuId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }

    public class SabitMenulerTranslate : BaseEntity
    {

        public string MenuAdi { get; set; }
        public string Aciklama { get; set; }
        public string BreadcrumbAdi { get; set; }
        public string BreadcrumbAciklama { get; set; }
        public string Url { get; set; }
        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int SabitMenuId { get; set; }
        public virtual SabitMenuler SabitMenuler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SabitMenulerTranslate>(entity =>
            {
                entity
               .Property(p => p.MenuAdi)
               .HasMaxLength(100);
            });
        }
    }


}