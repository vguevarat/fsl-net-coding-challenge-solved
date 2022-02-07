using System;

namespace HeyUrl.Dto.Url
{
    public class GetUrlResponseDto
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
