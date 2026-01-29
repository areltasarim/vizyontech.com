using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreViewModel
{
    public class UrunSiparisViewModel
    {
        [Required(ErrorMessage = "Ürün Adı alanı boş bırakılamaz..!")]
        public string UrunAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Marka alanı boş bırakılamaz..!")]
        public string Marka { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adet alanı boş bırakılamaz..!")]
        public int Adet { get; set; } = 0;

        [Required(ErrorMessage = "Fiyat alanı boş bırakılamaz..!")]
        public decimal Fiyat { get; set; } = decimal.Zero;
        public string Not { get; set; } = string.Empty;
        public int SiparisUrunId { get; set; }
        public IFormFile SayfaResmi { get; set; }
        //public IEnumerable<IFormFile> SayfaResmi { get; set; }
    }
}