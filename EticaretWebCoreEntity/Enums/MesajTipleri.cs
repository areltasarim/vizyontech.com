using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum MesajTipleri : int
    {
        Tumu,
        [Display(Name = "Gelen Mesajlar")] GelenMesajlar,
        [Display(Name = "Giden Mesajlar")] GidenMesajlar,
        [Display(Name = "Gelen Okunmamış Mesajlar")] GelenOkunmamisMesajlar,
        [Display(Name = "Gelen Silinmiş Mesajlar")] GelenSilinmisMesajlar,
        [Display(Name = "Giden Okunmamış Mesajlar")] GidenOkunmamisMesajlar,
        [Display(Name = "Giden Silinmiş Mesajlar")] GidenSilinmisMesajlar,
    }
}
