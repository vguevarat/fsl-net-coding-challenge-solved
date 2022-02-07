using System;

namespace HeyUrl.Dto.Click
{
    public class ListClicksByUrlResponseDto
    {
        public Guid UrlId { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
        public DateTime Date { get; set; }
    }
}
