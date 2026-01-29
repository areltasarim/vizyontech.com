using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum SSSDurumu : int
    {
        [Display(Name = "Hayır")] Hayir,
        [Display(Name = "Evet")] Evet,
    }
}
