using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using System;

namespace Tweetbook.SpecFlow.Drivers
{
    public class PostsDriver
    {
        private readonly HttpClient _httpClient;

        public PostsDriver(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AuthenticateAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "ego@ist.be",
                Password = "WhatTheFuck123_"
            });
            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }

        public async Task<HttpResponseMessage> CreatePost(CreatePostRequest postRequest)
        {
            return await _httpClient.PostAsJsonAsync(ApiRoutes.Posts.Create, postRequest);
        }

        public  async Task<HttpResponseMessage> DeletePost(Guid guid)
        {
            return await _httpClient.DeleteAsync(ApiRoutes.Posts.Delete.Replace("{postId}", guid.ToString()));
        }
    }
}
