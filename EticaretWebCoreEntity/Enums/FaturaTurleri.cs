using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum FaturaTurleri : int
    {
        [Display(Name = "Bireysel")] Bireysel = 0,
        [Display(Name = "Kurumsal")] Kurumsal = 1
    }
}
