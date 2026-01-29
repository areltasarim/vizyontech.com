using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum AdminSolMenuDurumlari : int
    {
        [Display(Name = "Gösterme")] Gosterme = 0,
        [Display(Name = "Göster")] Goster = 1
    }
}
