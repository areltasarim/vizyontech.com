using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum UyeDurumlari : int
    {
        [Display(Name = "Onay Bekliyor")] OnayBekliyor = 0,
        [Display(Name = "Onaylandı")] Onaylandi = 1, 
    }
}
