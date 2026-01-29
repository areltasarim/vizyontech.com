using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum SayfaDurumlari : int
    {
        [Display(Name = "Pasif")] Pasif = 0,
        [Display(Name = "Aktif")] Aktif = 1
    }
}
