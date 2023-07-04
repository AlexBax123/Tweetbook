namespace Tweetbook.Client.Models
{
    public class LoginResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; }=  new List<string>();
    }
}
