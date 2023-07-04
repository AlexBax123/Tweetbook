namespace Tweetbook.Client.Models
{
    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; }=  new List<string>();
    }
}
