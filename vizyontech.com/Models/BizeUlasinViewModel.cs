using System.ComponentModel.DataAnnotations;

namespace vizyontech.com.Models
{
    public class BizeUlasinViewModel
    {
        [Display(Name = "Firma Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string FirmaAdi { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string AdSoyad { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string Telefon { get; set; }

        [Display(Name = "Konu")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string Konu { get; set; }

        [Display(Name = "Mesaj")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz..!")]
        public string Mesaj { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "{0} boş bırakılamaz")]
        public bool? KVKK { get; set; }
        public string Captcha { get; set; }

    }
}
