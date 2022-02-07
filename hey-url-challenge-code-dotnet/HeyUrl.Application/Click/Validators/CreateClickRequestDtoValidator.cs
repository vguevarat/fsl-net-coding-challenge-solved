using FluentValidation;
using HeyUrl.Dto.Click;
using HeyUrl.Repository.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeyUrl.Application.Url.Validators
{
    public class CreateClickRequestDtoValidator : AbstractValidator<CreateClickRequestDto>
    {
        readonly IUrlRepository _urlRepository;
        public CreateClickRequestDtoValidator(
            IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;

            RuleFor(x => x).NotNull().WithMessage(Resources.Click.CreateClickRequestDtoRequired)
                .DependentRules(() =>
                {
                    RuleFor(x => x.UrlId).NotEmpty().WithMessage(Resources.Click.UrlIdRequired)
                     .DependentRules(() =>
                     {
                         RuleFor(x => x.UrlId).MustAsync(ValidateUrlExistenceAsync).WithMessage(Resources.Click.UrlNotFound);
                     });
                });
        }

        private async Task<bool> ValidateUrlExistenceAsync(CreateClickRequestDto request, Guid UrlId, CancellationToken cancellationToken)
        {
            if (!await _urlRepository.AnyAsync(x => x.Id == UrlId))
                return false;

            return true;
        }
    }
}
