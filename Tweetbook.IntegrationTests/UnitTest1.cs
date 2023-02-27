using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Xunit;

namespace Tweetbook.IntegrationTests
{

    public class UnitTest1
    {
        public readonly HttpClient _httpClient;

        public UnitTest1()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _httpClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task Test1Async()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", "1"));
        }
    }
}
