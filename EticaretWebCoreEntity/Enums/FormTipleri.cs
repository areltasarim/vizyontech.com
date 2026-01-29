using System.ComponentModel.DataAnnotations;

namespace EticaretWebCoreEntity.Enums
{
    public enum FormDurumlari
    {
        [Display(Name = "Evet")] Evet,
        [Display(Name = "Hayır")] Hayir,

    }

    public enum FormGenislikleri
    {
        [Display(Name = "Bir Sütun")] BirSutun,
        [Display(Name = "İki Sütun")] IkiSutun,
        [Display(Name = "Üç Sütun")] UcSutun,

    }

    public enum FormTurleri
    {
        [Display(Name = "Drop DownList")] DropDownList,
        [Display(Name = "Radio Buton")] RadioButon,
        [Display(Name = "CheckBox")] CheckBox,
        [Display(Name = "TexBox")] TexBox,
        [Display(Name = "TexArea")] TexArea,
        [Display(Name = "Dosya")] Dosya,
    }
}
