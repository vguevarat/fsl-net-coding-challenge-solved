using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HeyUrl.Application.Url;
using HeyUrl.Dto.Url;
using HeyUrl.Entity;
using HeyUrl.Repository.Abstraction;
using HeyUrl.UnitOfWork.Abstraction;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HeyUrl.Application.Test.Url
{
    [TestFixture]
    public class UrlApplicationTest
    {
        IConfiguration _configuration;
        Mock<IMapper> _mapperMock;
        Mock<IValidatorFactory> _validatorFactoryMock;
        Mock<IUnitOfWork> _unitOfWorkMock;
        Mock<IShortUrlRepository> _shortUrlRepositoryMock;
        Mock<IUrlRepository> _urlRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _validatorFactoryMock = new Mock<IValidatorFactory>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _shortUrlRepositoryMock = new Mock<IShortUrlRepository>();
            _urlRepositoryMock = new Mock<IUrlRepository>();

            var myConfiguration = new Dictionary<string, string>
            {
                {"Settings:MaxShortUrlLength", "5" },
                {"Settings:AvailableCharactesForShorturl", "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" },
            };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
        }

        [Test]
        public async Task CreateUrlResponseInvalidFromValidator()
        {
            var _createUrlRequestDtoValidatorMock = new Mock<IValidator<CreateUrlRequestDto>>();
            _createUrlRequestDtoValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUrlRequestDto>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure>
            {
                new("OriginalUrl", "The url is required")
            })));

            _validatorFactoryMock.Setup(x => x.GetValidator<CreateUrlRequestDto>()).Returns(_createUrlRequestDtoValidatorMock.Object);

            var application = new UrlApplication
                (_mapperMock.Object,
                _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _configuration,
                _shortUrlRepositoryMock.Object,
                _urlRepositoryMock.Object
                );


            var result = await application.CreateUrlAsync(new CreateUrlRequestDto());

            Assert.IsNotNull(result);
            Assert.True(!result.IsValid);
            Assert.True(result.Messages.Count(x => x.Type == Dto.Response.ApplicationMessageType.Error) == 1);
        }

        [Test]
        public async Task CreateUrlResponseInvalidShortUrl()
        {
            var _createUrlRequestDtoValidatorMock = new Mock<IValidator<CreateUrlRequestDto>>();
            _createUrlRequestDtoValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUrlRequestDto>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(new ValidationResult()));

            _validatorFactoryMock.Setup(x => x.GetValidator<CreateUrlRequestDto>()).Returns(_createUrlRequestDtoValidatorMock.Object);

            var _transactionMock = new Mock<IDbContextTransaction>();
            _transactionMock.Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>())).Returns(() => Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(async () => await Task.FromResult(_transactionMock.Object));

            _shortUrlRepositoryMock.Setup(x => x.GetLastShortUrlAsync()).Returns(() => Task.FromResult(new ShortUrl { Code = "ASDFGH" }));


            var application = new UrlApplication
                (_mapperMock.Object,
                _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _configuration,
                _shortUrlRepositoryMock.Object,
                _urlRepositoryMock.Object
                );


            var result = await application.CreateUrlAsync(new CreateUrlRequestDto());

            Assert.IsNotNull(result);
            Assert.True(!result.IsValid);
            Assert.True(result.Messages.Count(x => x.Type == Dto.Response.ApplicationMessageType.Error) == 1);
        }

        [Test]
        public async Task CreateUrlResponseValid()
        {
            var _createUrlRequestDtoValidatorMock = new Mock<IValidator<CreateUrlRequestDto>>();
            _createUrlRequestDtoValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUrlRequestDto>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(new ValidationResult()));
            _validatorFactoryMock.Setup(x => x.GetValidator<CreateUrlRequestDto>()).Returns(_createUrlRequestDtoValidatorMock.Object);
            var _transactionMock = new Mock<IDbContextTransaction>();
            _transactionMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(() => Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(async () => await Task.FromResult(_transactionMock.Object));

            _shortUrlRepositoryMock.Setup(x => x.GetLastShortUrlAsync()).Returns(() => Task.FromResult((ShortUrl)null));
            _shortUrlRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ShortUrl>())).Returns(() => Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(() => Task.FromResult(It.IsAny<int>()));
            _mapperMock.Setup(x => x.Map<Entity.Url>(It.IsAny<CreateUrlRequestDto>())).Returns(new Entity.Url());
            _mapperMock.Setup(x => x.Map<GetUrlResponseDto>(It.IsAny<Entity.Url>())).Returns(new GetUrlResponseDto { ShortUrl = "AAAAA" });


            var application = new UrlApplication
                (_mapperMock.Object,
                _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _configuration,
                _shortUrlRepositoryMock.Object,
                _urlRepositoryMock.Object
                );


            var result = await application.CreateUrlAsync(new CreateUrlRequestDto());

            Assert.IsNotNull(result);
            Assert.True(result.IsValid);
        }

        [Test]
        public async Task ListAllUrls()
        {
            IEnumerable<ListAllUrlResponseDto> urlList = new List<ListAllUrlResponseDto>
            {
                new()
                {
                    Id=Guid.NewGuid(),
                    OriginalUrl="http://www.google.com",
                    ShortUrl="ABCDE",
                    CreatedAt=DateTime.Now,
                    Count=1
                },
                 new()
                {
                    Id=Guid.NewGuid(),
                    OriginalUrl="http://www.yahoo.com",
                    ShortUrl="TREWQ",
                    CreatedAt=DateTime.Now,
                    Count=2
                },
            };
            _urlRepositoryMock.Setup(x => x.ListAllUrlsAsync()).Returns(() => Task.FromResult(urlList));

            var application = new UrlApplication
                (_mapperMock.Object,
                _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _configuration,
                _shortUrlRepositoryMock.Object,
                _urlRepositoryMock.Object
                );

            var result = await application.ListAllUrlsAsync();

            Assert.IsNotNull(result);
            Assert.True(result.Any());

        }

        [Test]
        public async Task GetUrlByShortUrl()
        {
            _urlRepositoryMock.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Entity.Url, bool>>>())).Returns(() => Task.FromResult(new Entity.Url()));
            _mapperMock.Setup(x => x.Map<GetUrlResponseDto>(It.IsAny<Entity.Url>())).Returns(new GetUrlResponseDto());

            var application = new UrlApplication
                (_mapperMock.Object,
                 _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _configuration,
                _shortUrlRepositoryMock.Object,
                _urlRepositoryMock.Object
                );

            var result = await application.GetUrlByShortUrlAsync("ABCDE");

            Assert.IsNotNull(result);

        }


        static object[] numberResults =
        {
            new object[] { 0,"AAAAA"},
            new object[] { 1,"AAAAB"},
            new object[] { 2,"AAAAC"},
            new object[] { 3,"AAAAD"},
            new object[] { 4,"AAAAE"},
            new object[] { 5,"AAAAF"},
        };
        [TestCaseSource(nameof(numberResults))]
        public void GenerateCode(int number, string result)
        {
            var maxShortUrlLength = _configuration.GetValue<int>("Settings:MaxShortUrlLength");
            var availableCharactesForShorturl = _configuration.GetValue<string>("Settings:AvailableCharactesForShorturl");
            var availableCharactes = availableCharactesForShorturl.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            var application = new UrlApplication
                (_mapperMock.Object,
                 _validatorFactoryMock.Object,
                _unitOfWorkMock.Object,
                _configuration,
                _shortUrlRepositoryMock.Object,
                _urlRepositoryMock.Object
                );

            var code = application.GenerateCode(number, maxShortUrlLength, availableCharactes);

            Assert.That(code == result);

        }

    }
}
