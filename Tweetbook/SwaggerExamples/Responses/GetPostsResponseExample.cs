using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using Tweetbook.Contracts.V1.Responses;

namespace Tweetbook.SwaggerExamples.Responses
{
    public class GetPostsResponseExample : IExamplesProvider<List<PostResponse>>
    {
        public List<PostResponse> GetExamples()
        {
            return new List<PostResponse>
            {
                new PostResponse
                {
                     Id = Guid.NewGuid(),
                      Name="The post name"
                      , UserId = "the guid of the creator", Tags = new List<TagResponse>
                      {
                          new TagResponse{ Id = Guid.NewGuid(), Name="tagname"},
                          new TagResponse{ Id = Guid.NewGuid(), Name="tagname2"},
                      }
                }
            };
        }
    }
}
