using AutoMapper;
using FluentValidation;
using HeyUrl.Application.Abstraction;
using HeyUrl.Application.Base;
using HeyUrl.Application.Url.Validators;
using HeyUrl.Dto.Response;
using HeyUrl.Dto.Url;
using HeyUrl.Repository.Abstraction;
using HeyUrl.UnitOfWork.Abstraction;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("HeyUrl.Application.Test")]
namespace HeyUrl.Application.Url
{
    public class UrlApplication : BaseApplication, IUrlApplication
    {
        readonly IMapper _mapper;
        readonly IValidatorFactory _validatorFactory;
        readonly IShortUrlRepository _shortUrlRepository;
        readonly IUrlRepository _urlRepository;

        readonly int _maxShortUrlLength;
        readonly List<string> _availableCharactesForShorturl;

        public UrlApplication(
            IMapper mapper,
            IValidatorFactory validatorFactory,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IShortUrlRepository shortUrlRepository,
            IUrlRepository urlRepository) : base(unitOfWork)
        {
            _mapper = mapper;
            _validatorFactory = validatorFactory;
            _shortUrlRepository = shortUrlRepository;
            _urlRepository = urlRepository;
            _maxShortUrlLength = configuration.GetValue<int>("Settings:MaxShortUrlLength");
            var availableCharactesForShortUrl = configuration.GetValue<string>("Settings:AvailableCharactesForShorturl");
            _availableCharactesForShorturl = availableCharactesForShortUrl.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public async Task<ResponseDto<CreateUrlResponseDto>> CreateUrlAsync(CreateUrlRequestDto request)
        {
            var response = new ResponseDto<CreateUrlResponseDto>();
            await ValidateRequestAsync(request, response);
            if (!response.IsValid)
                return response;

            using var transaction = (IDbContextTransaction)await _unitOfWork.BeginTransactionAsync();
            try
            {
                var shortUrl = await GenerateShortUrlAsync();
                if (string.IsNullOrWhiteSpace(shortUrl) || shortUrl.Length > 5)
                {
                    transaction.Rollback();
                    response.AddErrorResult(Resources.Url.ShortUrlNotAvailable);
                    return response;
                }

                var newUrl = _mapper.Map<Entity.Url>(request);
                newUrl.ShortUrl = shortUrl;
                await _urlRepository.AddAsync(newUrl);
                await _unitOfWork.CommitAsync();

                response.UpdateData(_mapper.Map<CreateUrlResponseDto>(newUrl));
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            return response;

        }

        public Task<IEnumerable<ListAllUrlResponseDto>> ListAllUrlsAsync()
        {
            return _urlRepository.ListAllUrlsAsync();
        }

        public async Task<GetUrlResponseDto> GetUrlByShortUrlAsync(string shortUrl)
        {
            return _mapper.Map<GetUrlResponseDto>(await _urlRepository.GetByAsync(x => x.ShortUrl == shortUrl));
        }

        private async Task ValidateRequestAsync(CreateUrlRequestDto request, ResponseDto response)
        {
            var validationResult = await _validatorFactory.GetValidator<CreateUrlRequestDto>().ValidateAsync(request);
            if (validationResult.IsValid)
                return;

            validationResult.Errors.ForEach(error =>
            {
                response.AddErrorResult(error.ErrorMessage);
            });

            await Task.CompletedTask;
        }

        private async Task<string> GenerateShortUrlAsync()
        {
            var lastShortUrl = await _shortUrlRepository.GetLastShortUrlAsync();
            if (lastShortUrl?.Code == null || lastShortUrl.Code.Length <= _maxShortUrlLength)
            {
                var shortUrl = new Entity.ShortUrl();
                await _shortUrlRepository.AddAsync(shortUrl);
                await _unitOfWork.CommitAsync();

                shortUrl.Code = GenerateCode(shortUrl.Id, _maxShortUrlLength, _availableCharactesForShorturl);
                return shortUrl.Code;
            }
            return null;
        }

        internal string GenerateCode(int number, int maxShortUrlLength, List<string> availableCharactesForCode)
        {
            if (number < 0 ||
               maxShortUrlLength < 1 ||
               (!availableCharactesForCode?.Any() ?? true))
                return null;

            var newNumberList = TransformNumberBase10ToBaseN(number, maxShortUrlLength, availableCharactesForCode.Count);
            return ChangeCharactersFromNumbers(newNumberList, availableCharactesForCode);
        }

        private List<int> TransformNumberBase10ToBaseN(int number, int maxShortUrlLength, int destinationBaseNumber)
        {
            var newNumberList = new List<int>();
            var dividend = number;
            do
            {
                var module = dividend % destinationBaseNumber;
                newNumberList.Add(module);
                dividend = dividend / destinationBaseNumber;
            } while (dividend != 0);

            if (newNumberList.Count < maxShortUrlLength)
                for (var index = newNumberList.Count; index < maxShortUrlLength; index++)
                    newNumberList.Add(0);

            return newNumberList;
        }

        private string ChangeCharactersFromNumbers(List<int> numberList, List<string> availableCharactesForCode)
        {
            var result = string.Empty;
            for (var index = 0; index < numberList.Count; index++)
                result = availableCharactesForCode[numberList[index]] + result;

            return result;
        }


    }
}
