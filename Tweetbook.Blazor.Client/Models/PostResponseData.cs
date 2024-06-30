using TweetbookApi;

namespace Tweetbook.Blazor.Client.Models
{
    public class PostResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();

        public PostResponseResponse PostResponseResponse { get; set; }
    }
}
