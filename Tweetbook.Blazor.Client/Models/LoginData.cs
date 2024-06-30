using System.ComponentModel.DataAnnotations;

namespace Tweetbook.Blazor.Client.Models
{
    public class LoginData
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
