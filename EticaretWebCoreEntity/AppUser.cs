using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity
{
    public class AppUser : IdentityUser<int>
    {

        public DateTime Tarih { get; set; }
        public UyeKayitTipi UyeKayitTipi { get; set; }
        public string Ad { get; set; }

        public string Soyad { get; set; }
        public string FirmaAdi { get; set; }
        public string CariKodu { get; set; }
        public int? OpakCariId { get; set; }
        public decimal CariLimit { get; set; }

        public int? PlasiyerId { get; set; }
        public virtual Plasiyer Plasiyer { get; set; }

        public string ProfilResmi { get; set; }

        public string VergiDairesi { get; set; }

        public long? VergiNumarasi { get; set; }

        public string Adres { get; set; }
        public string B2bSifre { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Gsm")]
        public string Gsm { get; set; }

        public decimal IskontoOrani { get; set; }
        public string VergiLevhasi { get; set; }

        public int? ParentId { get; set; }

        [Display(Name = "Üye Tipi")]
        public UyeTipleri Uyetipi { get; set; }

        [Display(Name = "İlçe")]
        public int? IlceId { get; set; }
        public virtual Ilceler Ilceler { get; set; }
        public string IpAdres { get; set; }

        public UyeDurumlari UyeDurumu { get; set; }
        public virtual ICollection<Adresler> Adresler { get; set; }
        public virtual ICollection<Mesajlar> Mesajlar { get; set; }
        public virtual ICollection<MesajKonulari> MesajKonulari { get; set; }
        public virtual ICollection<Yorumlar> Yorumlar { get; set; }
        public virtual ICollection<Sayfalar> Sayfalar { get; set; }
        public virtual ICollection<Sepet> Sepet { get; set; }
        public virtual ICollection<Siparisler> Siparisler { get; set; }
        public virtual ICollection<AlisverisListem> AlisverisListem { get; set; }
        public virtual ICollection<KuponToSiparis> KuponToSiparis { get; set; }
        public virtual ICollection<CariOdeme> CariOdeme { get; set; }
        public virtual ICollection<Logs> Logs { get; set; }


    }
}