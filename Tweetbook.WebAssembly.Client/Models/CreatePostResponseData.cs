namespace Tweetbook.WebAssembly.Client.Models
{
    public class CreatePostResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();
    }
}
