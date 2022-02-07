using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeyUrl.Entity
{
    public class Click
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Browser { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid UrlId { get; set; }
        [ForeignKey("UrlId")]
        public virtual Url Url { get; set; }
    }
}
