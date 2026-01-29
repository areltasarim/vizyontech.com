using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class UyeOlViewModel
    {

       

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Firma Adı")]
        public string FirmaAdi { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }
        public string ProfilResmi { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        public string B2bSifre { get; set; }
        public int? PlasiyerId { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Sabit Telefon")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        [Display(Name = "Gsm")]
        public string Gsm { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public UyeTipleri Uyetipi { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]

        public int? UlkeId { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        [Display(Name = "İl")]
        public int IlId { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        [Display(Name = "İlçe")]
        public int IlceId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Adres")]
        public string Adres { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        [DataType(DataType.MultilineText)]
        public string VergiLevhasi { get; set; }


        [DataType(DataType.Text)]
        public string VergiDairesi { get; set; }

        public long? VergiNumarasi { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal IskontoOrani { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal CariLimit { get; set; }

        [DataType(DataType.Text)]
        public string WebSite { get; set; }


        public UyeDurumlari UyeDurumu { get; set; }
        public Gender Cinsiyet { get; set; }

        public IFormFile UyeResim { get; set; }
        public int? ParentId { get; set; }

        public string ReturnUrl { get; set; }
        public string Captcha { get; set; }

        public AdresViewModel AdresEkle { get; set; } = new AdresViewModel();

        public List<RoleAssignViewModel> Roller { get; set; }


        [NotMapped]
        public IFormFile VergiLevhasiDosya { get; set; }

    }
}
