using FluentAssertions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Xunit;

namespace Tweetbook.IntegrationTests
{
    public class PostsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();
            // Act
            var response = await _httpClient.GetAsync(ApiRoutes.Posts.GetAll);
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PagedResponse<PostResponse>>()).Data.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistsInDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var testpostName = "MyTestPost1";
            var createResponse = await CreatePostAsync(new Contracts.V1.Requests.CreatePostRequest { Name = testpostName, Tags = new List<string> { "FU", "FU2" } });
            var createdPost = createResponse.Data;
            // Act
            var response = await _httpClient.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", createdPost.Id.ToString()));
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var post = await response.Content.ReadAsAsync<Response<PostResponse>>();
            post.Data.Id.Should().Be(createdPost.Id);
            post.Data.Name.Should().Be(testpostName);
        }
    }
}
