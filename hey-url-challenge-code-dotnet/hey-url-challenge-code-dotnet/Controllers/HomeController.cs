using HeyUrlChallengeCodeDotnet.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace hey_url_challenge_code_dotnet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error != null)
            {
                //TODO:Log error here
            }
            var vm = new ErrorViewModel() { RequestId = requestId };
            return View(vm);
        }
    }
}
