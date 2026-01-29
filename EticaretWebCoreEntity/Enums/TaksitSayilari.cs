using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum TaksitSayilari : int
    {
        [Display(Name = "Tek Çekim (Taksit Yok)")] TekCekim,
        [Display(Name = "2 Taksit'e Kadar")] ikiTaksit,
        [Display(Name = "3 Taksit'e Kadar")] UcTaksit,
        [Display(Name = "4 Taksit'e Kadar")] DortTaksit,
        [Display(Name = "5 Taksit'e Kadar")] BesTaksit,
        [Display(Name = "6 Taksit'e Kadar")] AltiTaksit,
        [Display(Name = "7 Taksit'e Kadar")] YediTaksit,
        [Display(Name = "8 Taksit'e Kadar")] SekizTaksit,
        [Display(Name = "9 Taksit'e Kadar")] DokuzTaksit,
        [Display(Name = "10 Taksit'e Kadar")] OnTaksit,
        [Display(Name = "11 Taksit'e Kadar")] OnBirTaksit,
        [Display(Name = "12 Taksit'e Kadar")] OnİkiTaksit,

    }
}
