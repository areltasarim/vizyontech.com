using EticaretWebCoreEntity;
using FluentValidation;

namespace EticaretWebCoreFluentValidation
{
    public class UrunValidator : AbstractValidator<UrunlerTranslate>
    {
        public UrunValidator()
        {
            RuleFor(x => x.UrunAdi)
                .NotEmpty()
                .WithMessage("Ürün Adı Boş Geçilemez!")
                .MinimumLength(3)
                .WithMessage("Ürün Adı Minimum 3 Karakter Olmalıdır!");
        }
    }
}
