using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EticaretWebCoreViewModel
{
    public class KasaViewModel
    {

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string TeslimatAd { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string TeslimatSoyad { get; set; }

        public string TeslimatFirmaAdi { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string TeslimatTelefon { get; set; }

        public string TeslimatEmail { get; set; }

        [Display(Name = "Ülke")]

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public int TeslimatUlkeId { get; set; }

        [Display(Name = "İl")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public int TeslimatIlId { get; set; }

        [Display(Name = "İlçe")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public int TeslimatIlceId { get; set; }



        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string TeslimatAdres { get; set; }



        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string FaturaAd { get; set; }


        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string FaturaSoyad { get; set; }

        public string FaturaTelefon { get; set; }
        public string FaturaEmail { get; set; }

        [Display(Name = "Ülke")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public int FaturaUlkeId { get; set; }


        [Display(Name = "İl")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public int FaturaIlId { get; set; }


        [Display(Name = "İlçe")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public int FaturaIlceId { get; set; }


        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string FaturaAdres { get; set; }

        public string FaturaFirmaAdi { get; set; }

        [Display(Name = "Vergi Dairesi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string VergiDairesi { get; set; }


        [Display(Name = "Vergi No")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public long VergiNumarasi { get; set; }

        public string SiparisNotu { get; set; }


        public int SiparisOdemeMetodId { get; set; }


        public bool UyeOlmakIstiyorum { get; set; } = false;
        public string Sifre { get; set; }

        public bool FaturaveTeslimatAdresiAynimi { get; set; } = false;


        [Display(Name = "Gizlilik İlkeleri")]
        [CheckBoxRequired(ErrorMessage = "Lütfen devam etmek için sözleşmeleri onaylayınız.")]
        public bool GizlilikIlkeleri { get; set; }
    }


}
