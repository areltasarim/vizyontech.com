using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum KargoMetodlari : int
    {
        [Display(Name = "Ücretsiz")] Ucretsiz = 1,
        [Display(Name = "Şartlı Ödeme")] SartliOdeme = 2,

    }

}
