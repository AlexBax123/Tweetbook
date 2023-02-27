using System.Collections.Generic;

namespace Tweetbook.Contracts.V1.Requests
{
    public class UpdatePostRequest
    {
        public string Name { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
