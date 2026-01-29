using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum BasvuruDurumlari : int
    {
        [Display(Name = "İncelenmedi")] Incelenmedi,
        [Display(Name = "İncelendi")] Incelendi,
    }

}
