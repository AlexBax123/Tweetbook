using TweetbookApi;

namespace Tweetbook.Client.Models
{
    public class RegisterData
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class RegisterResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();
    }
    public class AllPostsResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();

        public PostResponsePagedResponse PostResponsePagedResponse { get; set; }
    }

    public class PostResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();

        public PostResponseResponse PostResponseResponse { get; set; }
    }

    public class DeleteResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();

    }
}
