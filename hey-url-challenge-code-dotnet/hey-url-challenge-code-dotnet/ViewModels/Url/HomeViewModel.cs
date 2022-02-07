using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hey_url_challenge_code_dotnet.ViewModels.Url
{
    public class HomeViewModel
    {
        [Required]
        public string OriginalUrl { get; set; }

        public IEnumerable<ListUrlViewModel> Urls { get; set; }

    }
}
