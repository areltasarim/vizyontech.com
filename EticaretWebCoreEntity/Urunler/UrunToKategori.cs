using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class UrunToKategori : BaseEntity
    {


        //public bool AnaKategori { get; set; }

        public int KategoriId { get; set; }
        public virtual Kategoriler Kategoriler { get; set; }

        public int UrunId { get; set; }
        public virtual Urunler Urunler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            //builder.Entity<UrunToKategori>(entity =>
            //{
            //    entity
            //    .HasKey(p => new { p.KategoriId, p.UrunId });
            //});

            builder.Entity<UrunToKategori>(entity =>
            {
                entity
                .HasOne(p => p.Kategoriler)
                .WithMany(p => p.UrunToKategori)
                .HasForeignKey(p => p.KategoriId)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            builder.Entity<UrunToKategori>(entity =>
            {
                entity
                .HasOne(p => p.Urunler)
                .WithMany(p=> p.UrunToKategori)
                .HasForeignKey(p=> p.UrunId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            
        }
    }



}