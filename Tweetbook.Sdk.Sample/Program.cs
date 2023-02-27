using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests;

namespace Tweetbook.Sdk.Sample
{
    class Program
    {
        private const string UriString = "http://localhost:5001";

        static async Task Main(string[] args)
        {
            string cachedToken = string.Empty;
            var identityApi = RestService.For<IIdentityApi>(UriString);
            var tweetbookApi = RestService.For<ITweetbookApi>(UriString, new RefitSettings { 
             AuthorizationHeaderValueGetter = ()=> Task.FromResult(cachedToken)
            });

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sob@fu.com",
                Password = "Gfuself123_"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sob@fu.com",
                Password = "Gfuself123_"
            });

            cachedToken = loginResponse.Content.Token;

            var allPost = await tweetbookApi.GetAllPosts();
            var createdPost = await tweetbookApi.CreateAsync(new CreatePostRequest { Name = "whatever", Tags = new[] { "tag1", "tag2" } });

            var retrievedPost = await tweetbookApi.GetAsync(createdPost.Content.Id);
            var updatedPost = await tweetbookApi.UpdateAsync(createdPost.Content.Id, new UpdatePostRequest {
                Name = "whatever2",
                Tags = new[] { "tag1", "tag3" }
            });

            retrievedPost = await tweetbookApi.GetAsync(createdPost.Content.Id);

            var deletePost = await tweetbookApi.DeleteAsync(createdPost.Content.Id);
        }
    }
}
