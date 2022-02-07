using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeyUrl.Entity
{
    public class Url
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
        [Required]
        [StringLength(5)]
        public string ShortUrl { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Click> Clicks { get; set; } = new HashSet<Click>();
    }
}
