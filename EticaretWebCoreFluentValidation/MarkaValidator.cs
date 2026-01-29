using EticaretWebCoreEntity;
using FluentValidation;

namespace EticaretWebCoreFluentValidation
{
    public class MarkaValidator : AbstractValidator<Markalar>
    {
        public MarkaValidator()
        {
            RuleFor(x => x.MarkaAdi)
                .NotEmpty()
                .WithMessage("Marka Adı Boş Geçilemez!")
                .MinimumLength(3)
                .WithMessage("Marka Adı Minimum 3 Karakter Olmalıdır!");
        }
    }
}
