using FluentValidation;
using HeyUrl.Dto.Url;
using System;

namespace HeyUrl.Application.Url.Validators
{
    public class CreateUrlRequestDtoValidator : AbstractValidator<CreateUrlRequestDto>
    {
        public CreateUrlRequestDtoValidator()
        {

            RuleFor(x => x).NotNull().WithMessage(Resources.Url.CreateUrlRequestDtoRequired)
                .DependentRules(() =>
                {
                    RuleFor(x => x.OriginalUrl).NotEmpty().WithMessage(Resources.Url.OriginalUrlRequired)
                     .DependentRules(() =>
                     {
                         RuleFor(x => x.OriginalUrl).Must(ValidateUrlFormat).WithMessage(Resources.Url.UrlFormatInvalid);
                     });
                });
        }

        private bool ValidateUrlFormat(CreateUrlRequestDto request, string originalUrl)
        {
            try
            {
                if (!originalUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !originalUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    return false;

                var uri = new Uri(originalUrl);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
