using System;

namespace HeyUrl.Dto.Url
{
    public class CreateUrlResponseDto
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
