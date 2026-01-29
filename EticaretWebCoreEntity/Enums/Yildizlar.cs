using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum Yildizlar : int
    {
        [Display(Name = "Yıldız Yok")] YildizYok,
        [Display(Name = "Bir Yıldız")] BirYildiz,
        [Display(Name = "İki Yıldız")] İkiYildiz,
        [Display(Name = "Üç Yıldız")] UcYildiz,
        [Display(Name = "Dört Yıldız")] DortYildiz,
        [Display(Name = "Beş Yıldız")] BesYildiz,
    }
}
