using System;

namespace HeyUrl.Dto.Url
{
    public class ListAllUrlResponseDto
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Count { get; set; }
    }
}
