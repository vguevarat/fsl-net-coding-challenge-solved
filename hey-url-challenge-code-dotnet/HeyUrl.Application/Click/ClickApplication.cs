using AutoMapper;
using FluentValidation;
using HeyUrl.Application.Abstraction;
using HeyUrl.Application.Base;
using HeyUrl.Application.Url.Validators;
using HeyUrl.Dto.Click;
using HeyUrl.Dto.Response;
using HeyUrl.Dto.Url;
using HeyUrl.Repository.Abstraction;
using HeyUrl.UnitOfWork.Abstraction;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.Application.Click
{
    public class ClickApplication : BaseApplication, IClickApplication
    {
        readonly IMapper _mapper;
        readonly IValidatorFactory _validatorFactory;
        readonly IClickRepository _clickRepository;

        public ClickApplication(
            IMapper mapper,
            IValidatorFactory validatorFactory,
            IUnitOfWork unitOfWork,            
            IClickRepository clickRepository) : base(unitOfWork)
        {
            _mapper = mapper;
            _validatorFactory = validatorFactory;
            _clickRepository = clickRepository;
        }

        public async Task<ResponseDto<CreateClickResponseDto>> CreateClickAsync(CreateClickRequestDto request)
        {
            var response = new ResponseDto<CreateClickResponseDto>();
            await ValidateRequestASync(request, response);
            if (!response.IsValid)
                return response;

            var newClick = _mapper.Map<Entity.Click>(request);
            await _clickRepository.AddAsync(newClick);
            await _unitOfWork.CommitAsync();

            response.UpdateData(_mapper.Map<CreateClickResponseDto>(newClick));
            return response;
        }

        public async Task<IEnumerable<ListClicksByUrlResponseDto>> ListClickUrlCurrentMonthAsync(Guid urlId)
        {
            return await _clickRepository.ListClickUrlCurrentMonthAsync(urlId);
        }

        private async Task ValidateRequestASync(CreateClickRequestDto request, ResponseDto response)
        {
            var validationResult = await _validatorFactory.GetValidator<CreateClickRequestDto>().ValidateAsync(request);
            if (validationResult.IsValid)
                return;

            validationResult.Errors.ForEach(error =>
            {
                response.AddErrorResult(error.ErrorMessage);
            });
        }
    }
}
