using EticaretWebCoreEntity;
using FluentValidation;

namespace EticaretWebCoreFluentValidation
{
    public class SayfaValidator : AbstractValidator<SayfalarTranslate>
    {
        public SayfaValidator()
        {
            RuleFor(x => x.SayfaAdi)
                .NotEmpty()
                .WithMessage("Sayfa Adı Boş Geçilemez!")
                .MinimumLength(3)
                .WithMessage("Sayfa Adı Minimum 3 Karakter Olmalıdır!");
        }
    }
}
