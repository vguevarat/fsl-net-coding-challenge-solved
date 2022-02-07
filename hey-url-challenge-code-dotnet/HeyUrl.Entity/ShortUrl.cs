using System.ComponentModel.DataAnnotations;

namespace HeyUrl.Entity
{
    public class ShortUrl
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
    }
}
