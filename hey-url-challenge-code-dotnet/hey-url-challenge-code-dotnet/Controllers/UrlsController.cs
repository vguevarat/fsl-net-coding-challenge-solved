using AutoMapper;
using hey_url_challenge_code_dotnet.ViewModels;
using hey_url_challenge_code_dotnet.ViewModels.Url;
using HeyUrl.Application.Abstraction;
using HeyUrl.Dto.Click;
using HeyUrl.Dto.Url;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrlChallengeCodeDotnet.Controllers
{
    [Route("/")]
    public class UrlsController : Controller
    {
        readonly ILogger<UrlsController> _logger;
        readonly IMapper _mapper;
        readonly IBrowserDetector _browserDetector;
        readonly IUrlApplication _urlApplication;
        readonly IClickApplication _clickApplication;

        public UrlsController(
            ILogger<UrlsController> logger,
            IMapper mapper,
            IBrowserDetector browserDetector,
            IUrlApplication urlApplication,
            IClickApplication clickApplication
            )
        {
            _mapper = mapper;
            _browserDetector = browserDetector;
            _logger = logger;
            _urlApplication = urlApplication;
            _clickApplication = clickApplication;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await CreateHomeViewModelAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(await CreateHomeViewModelAsync(vm));

            var response = await _urlApplication.CreateUrlAsync(_mapper.Map<CreateUrlRequestDto>(vm));
            if (!response.IsValid)
            {
                response.Messages.ToList().ForEach(m =>
                {
                    ModelState.TryAddModelError("", m.Message);
                });

                return View(await CreateHomeViewModelAsync(vm));
            }

            TempData["NewShortUrl"] = response.Data.ShortUrl;
            return RedirectToAction(nameof(Index));
        }

        [Route("/{shortUrl}")]
        public async Task<IActionResult> Visit(string shortUrl)
        {
            var url = await _urlApplication.GetUrlByShortUrlAsync(shortUrl);
            if (url == null)
                return NotFound();

            await _clickApplication.CreateClickAsync(new CreateClickRequestDto
            {
                UrlId = url.Id,
                Browser = _browserDetector.Browser.Name,
                Platform = _browserDetector.Browser.OS
            });

            return Redirect(url.OriginalUrl);
        }


        [Route("urls/{shortUrl}")]
        public async Task<IActionResult> Show(string shortUrl)
        {
            var url = await _urlApplication.GetUrlByShortUrlAsync(shortUrl);
            if (url == null)
                return NotFound();

            var vm = _mapper.Map<ShowViewModel>(url);

            var urlClicks = await _clickApplication.ListClickUrlCurrentMonthAsync(url.Id);
            vm.DailyClicks = GetDailyClicks(urlClicks);
            vm.PlatformClicks = GetPlatformClicks(urlClicks);
            vm.BrowseClicks = GetBrowserClicks(urlClicks);
            return View(vm);
        }

        private async Task<HomeViewModel> CreateHomeViewModelAsync(HomeViewModel vm = null)
        {
            var model = vm ?? new HomeViewModel();
            var listAllUrls = await _urlApplication.ListAllUrlsAsync();
            model.Urls = _mapper.Map<IEnumerable<ListUrlViewModel>>(listAllUrls);
            return model;
        }

        private Dictionary<string, int> GetDailyClicks(IEnumerable<ListClicksByUrlResponseDto> clicks)
        {
            var result = new Dictionary<string, int>();
            var currentDate = DateTime.Now;
            var initialDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            do
            {
                var date = initialDate;
                result.Add(date.ToString("MM/dd"), clicks?.Count(x => x.Date.Date == date.Date) ?? 0);
                initialDate = initialDate.AddDays(1);
            } while (initialDate.Date <= currentDate.Date);

            return result;
        }

        private Dictionary<string, int> GetBrowserClicks(IEnumerable<ListClicksByUrlResponseDto> clicks)
        {
            return clicks?.GroupBy(x => x.Browser).ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<string, int>();
        }

        private Dictionary<string, int> GetPlatformClicks(IEnumerable<ListClicksByUrlResponseDto> clicks)
        {
            return clicks?.GroupBy(x => x.Platform).ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<string, int>();
        }
    }
}