namespace Tweetbook.WebAssembly.Client.Models
{
    public class DeleteResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();

    }
}
