using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum LogTipleri : int
    {
        [Display(Name = "Hata")] Hata,
        [Display(Name = "Bilgi")] Bilgi,
    }
}
