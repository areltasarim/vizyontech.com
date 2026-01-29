using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class SayfaFormu : BaseEntity
    {
        public string FirmaAdi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Konu { get; set; }
        public string Mesaj { get; set; }
        public string Resim { get; set; }
        public int? EntityId { get; set; }
        public SayfaFormTipleri SayfaFormTipi { get; set; }
        public DateTime Tarih { get; set; }


        //Özel Alanlar Projeye Göre Eklenir
        public string KonaklamaTipi { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime CikisTarihi { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SayfaFormu>(entity =>
            {
                entity
               .Property(p => p.Ad)
               .HasMaxLength(500);
            });
        }
    }
}