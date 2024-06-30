namespace Tweetbook.Blazor.Client.Models
{
    public class UpdatePost
    {
        public Guid PostId { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string UserId { get; set; }
    }
}
