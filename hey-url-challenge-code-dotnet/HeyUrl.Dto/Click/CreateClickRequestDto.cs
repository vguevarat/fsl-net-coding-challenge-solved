using System;

namespace HeyUrl.Dto.Click
{
    public class CreateClickRequestDto
    {
        public Guid UrlId { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
    }
}
