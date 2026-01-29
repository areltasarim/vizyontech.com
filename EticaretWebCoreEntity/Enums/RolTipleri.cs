using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum RolTipleri : int
    {
        [Display(Name = "Administrator")] Administrator = 1,
        [Display(Name = "Yönetici")] Yonetici = 2,
        [Display(Name = "Bayi")] Bayi = 3
    }
}
