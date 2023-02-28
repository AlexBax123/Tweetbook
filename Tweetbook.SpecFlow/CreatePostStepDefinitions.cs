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
using Tweetbook.SpecFlow.Drivers;

namespace Tweetbook.SpecFlow
{
    [Binding]
    public class CreatePostStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly PostsDriver _postsDriver;

        public CreatePostStepDefinitions(ScenarioContext scenarioContext
            , PostsDriver postsDriver)
        {
            _scenarioContext = scenarioContext;
            _postsDriver = postsDriver;
        }

        [Given(@"I am logged in")]
        public async Task GivenIAmLoggedInAsync()
        {
            await _postsDriver.AuthenticateAsync();
        }


        [When(@"I create a post ""([^""]*)"" with the following Tags")]
        public async Task WhenICreateAPostWithTheFollowingTags(string p0)
        {
            var testpostName = p0;
            _scenarioContext.Add("testpostName", testpostName);

            var postRequest = new Contracts.V1.Requests.CreatePostRequest { Name = testpostName, Tags = new List<string> { "FU", "FU2" } };
            HttpResponseMessage response = await _postsDriver.CreatePost(postRequest);
            _scenarioContext.Add("createdPost", response);
        }



        [Then(@"the post is created succesfully")]
        public async Task ThenThePostIsCreatedSuccesfullyAsync()
        {
            var createResponse = _scenarioContext.Get<HttpResponseMessage>("createdPost");
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var post = await createResponse.Content.ReadFromJsonAsync<Response<PostResponse>>();
            post.Data.Name.Should().Be(_scenarioContext.Get<string>("testpostName"));
            var location = createResponse.Headers.Location;
        }
    }
}
