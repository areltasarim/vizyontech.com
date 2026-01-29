using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class Menuler : BaseEntity
    {

        public int ParentMenuId { get; set; }
        public virtual Menuler ParentMenu { get; set; }

        public virtual ICollection<Menuler> AltMenuler { get; set; }

        public virtual ICollection<MenulerTranslate> MenulerTranslate { get; set; }

        public SeoUrlTipleri SeoUrlTipi { get; set; }
        [Required(ErrorMessage = "Bos Birakilamaz.")]

        public MenuTipleri MenuTipi { get; set; }

        public int? EntityId { get; set; }

        public int MenuKolon { get; set; }

        public MenuYerleri MenuYeri { get; set; }

        public MenuSekmeleri SekmeDurumu { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public int Sira { get; set; }


        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Menuler>(entity =>
            {
                entity
                .HasMany(p => p.MenulerTranslate )
                .WithOne(p => p.Menuler)
                .HasForeignKey(p => p. MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Menuler>(entity => {
                entity.HasMany(p => p.AltMenuler)
                .WithOne(p => p.ParentMenu)
                .HasForeignKey(p => p.ParentMenuId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //builder.Entity<SeoUrl>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Menuler)
            //    .WithOne(p => p.SeoUrl)
            //    .HasForeignKey(p => p.SeoUrlId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //});


            //builder.Entity<Sayfalar>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Menuler)
            //    .WithOne(p => p.Sayfalar)
            //    .HasForeignKey(p => p.SayfaId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //});

            //builder.Entity<Kategoriler>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Menuler)
            //    .WithOne(p => p.Kategoriler)
            //    .HasForeignKey(p => p.KategoriId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //});

            //builder.Entity<Urunler>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Menuler)
            //    .WithOne(p => p.Urunler)
            //    .HasForeignKey(p => p.UrunId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //});

            //builder.Entity<SabitMenuler>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Menuler)
            //    .WithOne(p => p.SabitMenuler)
            //    .HasForeignKey(p => p.SabitMenuId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //});
        }

    }

    public class MenulerTranslate : BaseEntity
    {

        public string MenuAdi { get; set; }
        public string Url { get; set; }
        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int MenuId { get; set; }
        public virtual Menuler Menuler { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<MenulerTranslate>(entity =>
            {
                entity
               .Property(p => p.MenuAdi)
               .HasMaxLength(100);
            });
        }
    }


}