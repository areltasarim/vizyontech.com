using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
   public enum UrunSecenekTipleri
    {
        [Display(Name = "Seçenek")]
        Secenek = 0,
        [Display(Name = "Radio")]
        Radio = 1,
        [Display(Name = "Chekbox")]
        Chekbox = 2,
        [Display(Name = "Textbox")]
        Textbox = 3,
        [Display(Name = "TextArea")]
        TextArea = 4,
        [Display(Name = "Dosya")]
        Dosya = 5,
        [Display(Name = "Tarih")]
        Tarih = 6
    }



}
