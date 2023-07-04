namespace Tweetbook.Client.Models
{
    public class Post
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public IEnumerable<string> Tags { get; set; }

    }


    public class Tag
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }
    }
}
