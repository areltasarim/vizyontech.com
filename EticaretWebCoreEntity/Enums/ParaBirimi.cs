using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum ParaBirimi : int
    {
        [Display(Name = "TL")] TRY = 1,
        [Display(Name = "USD")] USD = 2,
        [Display(Name = "EURO")] EUR = 3,
    }

}






