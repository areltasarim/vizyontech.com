using EticaretWebCoreEntity;
using FluentValidation;

namespace EticaretWebCoreFluentValidation
{
    public class KategoriValidator : AbstractValidator<KategorilerTranslate>
    {
        public KategoriValidator()
        {
            RuleFor(x => x.KategoriAdi)
                .NotEmpty()
                .WithMessage("Kategori Adı Boş Geçilemez!")
                .MinimumLength(3)
                .WithMessage("Kategori Adı Minimum 3 Karakter Olmalıdır!");
        }
    }
}
