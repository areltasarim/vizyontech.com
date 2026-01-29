using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum SepetAdetGuncellemeDurumlari : int
    {
        [Display(Name = "Arttır")] Arttir,
        [Display(Name = "Azalt")] Azalt,
    }
}
