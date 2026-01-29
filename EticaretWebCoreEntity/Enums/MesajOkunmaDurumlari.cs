using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum MesajOkunmaDurumlari : int
    {
        [Display(Name = "Okunmadı")] Okunmadi = 0,
        [Display(Name = "Okundu")] Okundu = 1,
        [Display(Name = "Silindi")] Silindi = 2
    }
}
