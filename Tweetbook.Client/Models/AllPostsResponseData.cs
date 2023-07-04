using TweetbookApi;

namespace Tweetbook.Client.Models
{
    public class AllPostsResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();

        public PostResponsePagedResponse PostResponsePagedResponse { get; set; }
    }
}
