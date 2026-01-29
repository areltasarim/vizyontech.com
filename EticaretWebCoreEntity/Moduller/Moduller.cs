using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace EticaretWebCoreEntity
{
    public class Moduller : BaseEntity
    {

        public virtual ICollection<ModullerTranslate> ModullerTranslate { get; set; } = new List<ModullerTranslate>();

        public ModulTipleri ModulTipi { get; set; }
        public int EntityId { get; set; }
        public int Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Moduller>(entity =>
            {
                entity
              .HasMany(p => p.ModullerTranslate)
              .WithOne(p => p.Moduller)
              .HasForeignKey(p => p.ModulId)
              .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
    public class ModullerTranslate : BaseEntity
    {
        public string ModulAdi { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int ModulId { get; set; }
        public virtual Moduller Moduller { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<ModullerTranslate>(entity =>
            {
                entity
               .Property(p => p.ModulAdi)
               .HasMaxLength(500);
            });
        }
    }

}