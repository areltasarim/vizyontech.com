using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum MenuYerleri : int
    {
        [Display(Name = "Üst Menü")] UstMenu = 1,
        [Display(Name = "Üst Menü 2")] UstMenu2 = 2,
        [Display(Name = "Accordion Menü")] AccordionMenu = 3,
        [Display(Name = "Accordion Footer Menü")] AccordionFooterMenu = 4,
        [Display(Name = "Footer Kurumsal")] FooterMenuSol = 5,
        [Display(Name = "Footer Güvenlik")] FooterMenuOrta = 6,
        [Display(Name = "Footer Teknik Destek")] FooterMenuSag = 7,
        [Display(Name = "Footer Menü Alt")] FooterMenuAlt = 8,
    }
}
