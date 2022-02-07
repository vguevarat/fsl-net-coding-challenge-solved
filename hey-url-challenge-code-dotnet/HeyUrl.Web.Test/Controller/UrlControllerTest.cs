using AutoMapper;
using hey_url_challenge_code_dotnet.ViewModels;
using hey_url_challenge_code_dotnet.ViewModels.Url;
using HeyUrl.Application.Abstraction;
using HeyUrl.Dto.Click;
using HeyUrl.Dto.Response;
using HeyUrl.Dto.Url;
using HeyUrlChallengeCodeDotnet.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyUrl.Web.Test.Controller
{
    [TestFixture]
    public class UrlControllerTest
    {
        Mock<IMapper> _mockIMapper;
        Mock<IBrowserDetector> _mockIBrowserDetector;
        Mock<IUrlApplication> _mockIUrlApplication;
        Mock<IClickApplication> _mockIClickApplication;


        [SetUp]
        public void Setup()
        {
            _mockIMapper = new Mock<IMapper>();
            _mockIBrowserDetector = new Mock<IBrowserDetector>();
            _mockIUrlApplication = new Mock<IUrlApplication>();
            _mockIClickApplication = new Mock<IClickApplication>();
        }

        [Test]
        public async Task IndexReturnsView()
        {
            _mockIUrlApplication.Setup(x => x.ListAllUrlsAsync()).Returns(() => Task.FromResult(It.IsAny<IEnumerable<ListAllUrlResponseDto>>()));
            _mockIMapper.Setup(x => x.Map<IEnumerable<ListUrlViewModel>>(It.IsAny<IEnumerable<ListAllUrlResponseDto>>())).Returns(It.IsAny<IEnumerable<ListUrlViewModel>>());

            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );

            var actionResult = await controller.Index();

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(ViewResult));

        }

        [Test]
        public async Task PostIndexReturnsViewModelStateInvalid()
        {
            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );
            controller.ModelState.AddModelError("OriginalUrl", "the Url is required.");

            var actionResult = await controller.Index();

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(ViewResult));

        }

        [Test]
        public async Task PostIndexReturnsRedirectToIndex()
        {
            _mockIMapper.Setup(x => x.Map<CreateUrlRequestDto>(It.IsAny<HomeViewModel>())).Returns(It.IsAny<CreateUrlRequestDto>());
            _mockIUrlApplication.Setup(x => x.CreateUrlAsync(It.IsAny<CreateUrlRequestDto>())).Returns(() => Task.FromResult(new ResponseDto<CreateUrlResponseDto>()));

            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );

            var actionResult = await controller.Index(new HomeViewModel { OriginalUrl = "http://www.google.com" });

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(RedirectToActionResult));

        }

        [Test]
        public async Task PostIndexReturnsViewErrorReponse()
        {
            _mockIMapper.Setup(x => x.Map<CreateUrlRequestDto>(It.IsAny<HomeViewModel>())).Returns(It.IsAny<CreateUrlRequestDto>());

            var response = new ResponseDto<CreateUrlResponseDto>();
            response.AddErrorResult("Short url not available.");
            _mockIUrlApplication.Setup(x => x.CreateUrlAsync(It.IsAny<CreateUrlRequestDto>())).Returns(() => Task.FromResult(response));
            _mockIUrlApplication.Setup(x => x.ListAllUrlsAsync()).Returns(() => Task.FromResult(It.IsAny<IEnumerable<ListAllUrlResponseDto>>()));
            _mockIMapper.Setup(x => x.Map<IEnumerable<ListUrlViewModel>>(It.IsAny<IEnumerable<ListAllUrlResponseDto>>())).Returns(It.IsAny<IEnumerable<ListUrlViewModel>>());


            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );

            var actionResult = await controller.Index(new HomeViewModel { OriginalUrl = "http://www.google.com" });

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(ViewResult));

        }

        [Test]
        public async Task VisitReturnsNotFound()
        {
            _mockIUrlApplication.Setup(x => x.GetUrlByShortUrlAsync(It.IsAny<string>())).Returns(() => Task.FromResult(It.IsAny<GetUrlResponseDto>()));

            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );

            var actionResult = await controller.Visit("ABCDE");

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(NotFoundResult));

        }


        [Test]
        public async Task VisitReturnsRedirect()
        {
            _mockIUrlApplication.Setup(x => x.GetUrlByShortUrlAsync(It.IsAny<string>())).Returns(() => Task.FromResult(new GetUrlResponseDto() { OriginalUrl = "http://www.google.com" }));
            _mockIClickApplication.Setup(x => x.CreateClickAsync(It.IsAny<CreateClickRequestDto>())).Returns(() => Task.FromResult(It.IsAny<ResponseDto<CreateClickResponseDto>>()));
            _mockIBrowserDetector.Setup(x => x.Browser.Name).Returns(It.IsAny<string>());
            _mockIBrowserDetector.Setup(x => x.Browser.OS).Returns(It.IsAny<string>());

            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );

            var actionResult = await controller.Visit("ABCDE");

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(RedirectResult));

        }

        [Test]
        public async Task ShowReturnsNotFound()
        {
            _mockIUrlApplication.Setup(x => x.GetUrlByShortUrlAsync(It.IsAny<string>())).Returns(() => Task.FromResult(It.IsAny<GetUrlResponseDto>()));

            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );

            var actionResult = await controller.Show("ABCDE");

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(NotFoundResult));

        }

        [Test]
        public async Task ShowReturnsRedirect()
        {
            _mockIUrlApplication.Setup(x => x.GetUrlByShortUrlAsync(It.IsAny<string>())).Returns(() => Task.FromResult(new GetUrlResponseDto() { OriginalUrl = "http://www.google.com" }));
            _mockIMapper.Setup(x => x.Map<ShowViewModel>(It.IsAny<GetUrlResponseDto>())).Returns(new ShowViewModel());
            _mockIClickApplication.Setup(x => x.ListClickUrlCurrentMonthAsync(It.IsAny<Guid>())).Returns(() => Task.FromResult(It.IsAny<IEnumerable<ListClicksByUrlResponseDto>>()));
            
            var controller = new UrlsController
                (null,
                _mockIMapper.Object,
                _mockIBrowserDetector.Object,
                _mockIUrlApplication.Object,
                _mockIClickApplication.Object
                );

            var actionResult = await controller.Show("ABCDE");

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.GetType() == typeof(ViewResult));

        }
    }
}
