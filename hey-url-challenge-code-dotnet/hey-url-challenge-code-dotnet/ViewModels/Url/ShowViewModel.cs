using System;
using System.Collections.Generic;

namespace hey_url_challenge_code_dotnet.ViewModels
{
    public class ShowViewModel
    {
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, int> DailyClicks { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> BrowseClicks { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> PlatformClicks { get; set; } = new Dictionary<string, int>();
    }
}