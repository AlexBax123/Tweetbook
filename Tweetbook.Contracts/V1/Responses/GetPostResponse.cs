using System;
using System.Collections.Generic;

namespace Tweetbook.Contracts.V1.Responses
{
    public class GetPostResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }

    public class GetTagResponse
    {

    }
}
