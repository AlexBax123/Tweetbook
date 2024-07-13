namespace Tweetbook.WebAssembly.Client.Models
{
    public class RegisterResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();
    }
}
