using System.ComponentModel.DataAnnotations;

namespace Tweetbook.Client.Models
{
    public class UpdatePost
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Tags { get; set; }
        public string UserId { get; set; }
    }
}
