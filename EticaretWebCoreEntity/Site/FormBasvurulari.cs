using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class FormBasvurulari : BaseEntity
    {
        public virtual ICollection<FormCevaplari> FormCevaplari { get; set; }

        public BasvuruDurumlari BasvuruDurumu { get; set; }
        public int SayfaId { get; set; }
        public DateTime BasvuruTarihi { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<FormBasvurulari>(entity =>
            {
               

            });
        }
    }


}