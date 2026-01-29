using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EticaretWebCoreEntity
{
    public class SiteAyarlari : BaseEntity
    {
        public virtual ICollection<SiteAyarlariTranslate> SiteAyarlariTranslate { get; set; }
        public virtual ICollection<AdresBilgileri> AdresBilgileri { get; set; }


        public int AktifDilId { get; set; }
        public virtual Diller AktifDil { get; set; }

        public int ParaBirimId { get; set; }
        public virtual ParaBirimleri AktifParaBirimi { get; set; }

        public string FirmaAdi { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Pinterest { get; set; }
        public string GooglePlus { get; set; }
        public string Youtube { get; set; }
        public string Whatsapp { get; set; }

        [DataType(DataType.MultilineText)]
        public string HeaderKod { get; set; }

        [DataType(DataType.MultilineText)]
        public string BodyKod { get; set; }
        public string FooterKod { get; set; }
        public string UstLogo { get; set; }
        public string FooterLogo { get; set; }
        public string MobilLogo { get; set; }
        public string Favicon { get; set; }
        public string MailLogo { get; set; }
        public SayfaDurumlari PopupDurum { get; set; }

        public string EmailHost { get; set; }
        public string EmailAdresi { get; set; }
        public string EmailSifre { get; set; }
        public int EmailPort { get; set; }
        public bool EmailSSL { get; set; }
        public string GonderilecekMail { get; set; }
        public MailTipleri MailTipi { get; set; }
        public string MailBaslik { get; set; }
        public string MailKonu { get; set; }
        public string MailGonderildiMesaji { get; set; }
        public ExchangeVersion ExchangeVersiyon { get; set; }

        //Eğer bu seçenek aktif olursa Bir kategorinin alt kategorisi varsa o alt kategoriler listelenir.
        //Eğer aktif değilse direk tıklanan kategorinin ürünleri listelenir
        public SayfaDurumlari SinirsiKategoriDurum { get; set; }

        [NotMapped]
        public string EncrypedId { get; set; }

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<SiteAyarlari>(entity =>
            {
                entity
                .HasMany(p => p.SiteAyarlariTranslate)
                .WithOne(p => p.SiteAyarlari)
                .HasForeignKey(p => p.SiteAyarId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

    public class SiteAyarlariTranslate : BaseEntity
    {

        public string MetaBaslik { get; set; }
        public string MetaAciklama { get; set; }
        public string MetaAnahtar { get; set; }

        [DataType(DataType.MultilineText)]
        public string HeaderAciklama { get; set; }
        [DataType(DataType.MultilineText)]
        public string FooterAciklama { get; set; }
        public string Popup { get; set; }

        public int DilId { get; set; }
        public virtual Diller Diller { get; set; }

        public int SiteAyarId { get; set; }
        public virtual SiteAyarlari SiteAyarlari { get; set; }

        public override void Build(ModelBuilder builder)
        {

            builder.Entity<SiteAyarlariTranslate>(entity =>
            {
                entity
               .Property(p => p.MetaBaslik)
               .HasMaxLength(255);
            });
        }
    }
}