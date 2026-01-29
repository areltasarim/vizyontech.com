using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EticaretWebCoreEntity
{
    public class Adresler : BaseEntity
    {
        [Display(Name = "Üye")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int UyeId { get; set; }
        public virtual AppUser Uyeler { get; set; }

        [Display(Name = "İlçe")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int IlceId { get; set; }
        public virtual Ilceler Ilceler { get; set; }

        [Display(Name = "Adres Adı")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage = "{0} en fazla 100 karakter olabilir")]
        public string AdresAdi { get; set; } = "";


        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage = "{0} en fazla 100 karakter olabilir")]
        public string Ad { get; set; } = "";


        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage = "{0} en fazla 100 karakter olabilir")]
        public string Soyad { get; set; } = "";

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "{0} en fazla 500 karakter olabilir")]
        public string Adres { get; set; }

        [Display(Name = "Fatura Adres")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "{0} en fazla 500 karakter olabilir")]
        public string FaturaAdres { get; set; }

        [Display(Name = "Posta Kodu")]
        public string PostaKodu { get; set; }

        public string Telefon { get; set; }

        [Display(Name = "Gsm")]
        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "{0} en fazla 15 karakter olabilir")]
        public string Gsm { get; set; }

        [Display(Name = "Firma Adı")]
        [DataType(DataType.Text)]
        [MaxLength(200, ErrorMessage = "{0} en fazla 200 karakter olabilir")]
        public string FirmaAdi { get; set; }

        [Display(Name = "Vergi Dairesi")]
        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage = "{0} en fazla 100 karakter olabilir")]
        public string VergiDairesi { get; set; }

        [Display(Name = "Vergi No")]
        public long? VergiNumarasi { get; set; }
        public string VergiLevhasi { get; set; }

        public FaturaTurleri FaturaTuru { get; set; }
        public virtual ICollection<Siparisler> Siparisler { get; set; }

        public override void Build(ModelBuilder builder)
        {

            builder.Entity<Adresler>(entity =>
            {
       
            });

            builder.Entity<AppUser>(entity =>
            {


                entity
                .HasMany(p => p.Adresler)
                .WithOne(p => p.Uyeler)
                .HasForeignKey(p => p.UyeId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            //builder.Entity<Ulkeler>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Adresler)
            //    .WithOne(p => p.Ulkeler)
            //    .HasForeignKey(p => p.UlkeId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //});
            //builder.Entity<Iller>(entity =>
            //{
            //    entity
            //    .HasMany(p => p.Adresler)
            //    .WithOne(p => p.Iller)
            //    .HasForeignKey(p => p.IlId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //});
            builder.Entity<Ilceler>(entity =>
            {
                entity
                .HasMany(p => p.Adresler)
                .WithOne(p => p.Ilceler)
                .HasForeignKey(p => p.IlceId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }


}