using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum KapakTipleri : int
    {
        [Display(Name = "Ciltli")] Ciltli,
        [Display(Name = "Ciltsiz")] Ciltsiz,
        [Display(Name = "İnce Kapak")] InceKapak
    }
}
