using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;

namespace Tweetbook.SpecFlow.Steps
{
    [Binding]
    public class CreatePostSteps//: CustomWebApplicationFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        private HttpClient _client { get; set; }
        public CreatePostSteps(ScenarioContext scenarioContext
            , HttpClient httpClient)
        {
            _scenarioContext = scenarioContext;
            _httpClient = httpClient;
        }

        [Given(@"I am logged in")]
        public async Task GivenIAmLoggedInAsync()
        {
            await AuthenticateAsync();
        }
        protected async Task AuthenticateAsync()
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
            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }

        [Given(@"the post exists in the system")]
        public async Task GivenThePostExistsInTheSystem(Table table)
        {
            await WhenICreateAPostWithTheFollowingTagsAsync();
        }


        
        [When(@"I create a post with the following Tags")]
        public async Task WhenICreateAPostWithTheFollowingTagsAsync()
        {
            var testpostName = "MyTestPost1";
            var postRequest = new Contracts.V1.Requests.CreatePostRequest { Name = testpostName, Tags = new List<string> { "FU", "FU2" } };
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Posts.Create, postRequest);
            _scenarioContext.Add("createdPost", response);
        }

        [When(@"the post is deleted")]
        public async Task WhenThePostIsDeleted()
        {
            var createResponse = _scenarioContext.Get<HttpResponseMessage>("createdPost");
            var post = await createResponse.Content.ReadAsAsync<Response<PostResponse>>();
            var guid = post.Data.Id;
            var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Posts.Delete.Replace("{postId}", guid.ToString()));
            _scenarioContext.Add("deleteResponse", deleteResponse);
        }

        [Then(@"the post is created succesfully")]
        public async Task ThenThePostIsCreatedSuccesfullyAsync()
        {
            var createResponse = _scenarioContext.Get<HttpResponseMessage>("createdPost");
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var post = await createResponse.Content.ReadAsAsync<Response<PostResponse>>();
            post.Data.Name.Should().Be("MyTestPost1");
            var location = createResponse.Headers.Location;
        }
        
        [Then(@"the post is deleted succesfully")]
        public void ThenThePostIsDeletedSuccesfully()
        {
            var deleteResponse =_scenarioContext.Get<HttpResponseMessage>("deleteResponse");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
