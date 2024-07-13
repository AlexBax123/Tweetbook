using System.ComponentModel.DataAnnotations;

namespace Tweetbook.WebAssembly.Client.Models
{
    public class CreatePost
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Tags { get; set; }
    }
}
