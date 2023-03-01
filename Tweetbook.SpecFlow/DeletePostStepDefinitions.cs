using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.SpecFlow.Drivers;
using FluentAssertions;
using System.Net;

namespace Tweetbook.SpecFlow
{
    [Binding]
    public class DeletePostStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly PostsDriver _postsDriver;

        public DeletePostStepDefinitions(ScenarioContext scenarioContext
            , PostsDriver postsDriver)
        {
            _scenarioContext = scenarioContext;
            _postsDriver = postsDriver; 

        }

        [Given(@"I am logged in as Deleter")]
        public async Task GivenIAmLoggedInAsDeleter()
        {
            await _postsDriver.AuthenticateAsync();
        }

        [Given(@"the post ""([^""]*)"" exists in the system")]
        public async Task GivenThePostExistsInTheSystem(string p0)
        {
            await WhenICreateAPostWithTheFollowingTags(p0);
        }

        public async Task WhenICreateAPostWithTheFollowingTags(string p0)
        {
            var testpostName = p0;
            _scenarioContext.Add("testpostName", testpostName);

            var postRequest = new Contracts.V1.Requests.CreatePostRequest { Name = testpostName, Tags = new List<string> { "FU", "FU2" } };
            HttpResponseMessage response = await _postsDriver.CreatePost(postRequest);
            _scenarioContext.Add("createdPost", response);
        }

        [When(@"the post is deleted")]
        public async Task WhenThePostIsDeleted()
        {
            var createResponse = _scenarioContext.Get<HttpResponseMessage>("createdPost");
            var post = await createResponse.Content.ReadFromJsonAsync<Response<PostResponse>>();
            var guid = post.Data.Id;
            var deleteResponse = await _postsDriver.DeletePost(guid);
            _scenarioContext.Add("deleteResponse", deleteResponse);
        }



        [Then(@"the post is deleted succesfully")]
        public void ThenThePostIsDeletedSuccesfully()
        {
            var deleteResponse = _scenarioContext.Get<HttpResponseMessage>("deleteResponse");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }



    }
}
