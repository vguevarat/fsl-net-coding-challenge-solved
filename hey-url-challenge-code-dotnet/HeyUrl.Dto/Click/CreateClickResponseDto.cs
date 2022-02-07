using System;

namespace HeyUrl.Dto.Click
{
    public class CreateClickResponseDto
    {
        public Guid Id { get; set; }
        public Guid UrlId { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
        public DateTime Date { get; set; }
    }
}
