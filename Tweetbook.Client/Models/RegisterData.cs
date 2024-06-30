using System.ComponentModel.DataAnnotations;

namespace Tweetbook.Client.Models
{
    public class RegisterData
    {
        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
