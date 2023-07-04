namespace Tweetbook.Client.Models
{
    public class CreatePost
    {
        public string Name { get; set; }
        public string Tags { get; set; }
    }

    public class UpdatePost
    {
        public Guid PostId { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePostResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();
    }

    public class UpdatePostResponseData
    {
        public bool Success { get; internal set; }
        public ICollection<string> Errors { get; internal set; } = new List<string>();
    }
}
