using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HeyUrl.Application.Click;
using HeyUrl.Dto.Click;
using HeyUrl.Repository.Abstraction;
using HeyUrl.UnitOfWork.Abstraction;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HeyUrl.Application.Test.Click
{
    [TestFixture]
    public class ClickApplicationTest
    {
        Mock<IMapper> _mapperMock;
        Mock<IValidatorFactory> _validatorFactoryMock;
        Mock<IUnitOfWork> _unitOfWorkMock;
        Mock<IClickRepository> _clickRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _validatorFactoryMock = new Mock<IValidatorFactory>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _clickRepositoryMock = new Mock<IClickRepository>();
        }

        [Test]
        public async Task CreateClickResponseInvalidFromValidator()
        {
            var _createClickRequestDtoValidatorMock = new Mock<IValidator<CreateClickRequestDto>>();
            _createClickRequestDtoValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateClickRequestDto>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure>
            {
                new("", "Url is not found.")
            })));
            _validatorFactoryMock.Setup(x => x.GetValidator<CreateClickRequestDto>()).Returns(_createClickRequestDtoValidatorMock.Object);

            var application = new ClickApplication
                (_mapperMock.Object,
                _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _clickRepositoryMock.Object
                );


            var result = await application.CreateClickAsync(new CreateClickRequestDto());

            Assert.IsNotNull(result);
            Assert.True(!result.IsValid);
            Assert.True(result.Messages.Count(x => x.Type == Dto.Response.ApplicationMessageType.Error) == 1);
        }

        [Test]
        public async Task CreateClickResponseValid()
        {
            var _createClickRequestDtoValidatorMock = new Mock<IValidator<CreateClickRequestDto>>();
            _createClickRequestDtoValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateClickRequestDto>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(new ValidationResult()));
            _validatorFactoryMock.Setup(x => x.GetValidator<CreateClickRequestDto>()).Returns(_createClickRequestDtoValidatorMock.Object);
            _mapperMock.Setup(x => x.Map<Entity.Click>(It.IsAny<CreateClickRequestDto>())).Returns(It.IsAny<Entity.Click>());
            _clickRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Entity.Click>())).Returns(() => Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(() => Task.FromResult(It.IsAny<int>()));
            _mapperMock.Setup(x => x.Map<CreateClickResponseDto>(It.IsAny<Entity.Click>())).Returns(new CreateClickResponseDto());

            var application = new ClickApplication
                (_mapperMock.Object,
                _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _clickRepositoryMock.Object
                );


            var result = await application.CreateClickAsync(new CreateClickRequestDto());

            Assert.IsNotNull(result);
            Assert.True(result.IsValid);
        }

        [Test]
        public async Task ListClickUrlCurrentMonth()
        {
            IEnumerable<ListClicksByUrlResponseDto> resultMock = new List<ListClicksByUrlResponseDto>
            {
                new()
                {
                    Browser="Chrome",
                    Date=DateTime.Now,
                    Platform="Windows",
                    UrlId=Guid.NewGuid()
                }
            };
            _clickRepositoryMock.Setup(x => x.ListClickUrlCurrentMonthAsync(It.IsAny<Guid>())).Returns(() => Task.FromResult(resultMock));

            var application = new ClickApplication
                (_mapperMock.Object,
                _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _clickRepositoryMock.Object
                );


            var result = await application.ListClickUrlCurrentMonthAsync(Guid.NewGuid());

            Assert.IsNotNull(result);
            Assert.True(result.Any());
        }
    }
}
