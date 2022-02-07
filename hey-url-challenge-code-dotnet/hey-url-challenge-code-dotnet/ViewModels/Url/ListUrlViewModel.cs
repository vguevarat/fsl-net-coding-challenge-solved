using System;

namespace hey_url_challenge_code_dotnet.ViewModels.Url
{
    public class ListUrlViewModel
    {
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Count { get; set; }
    }
}
