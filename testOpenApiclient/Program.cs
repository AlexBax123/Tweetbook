// See https://aka.ms/new-console-template for more information
using Polly;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using TweetbookApi;

Console.WriteLine("Hello, World!");

using var httpClient = new HttpClient();
const string UriString = "http://localhost:5001";// from the tweetbook sln

var apiClient = new TweetbookApi.Tweetbookservice(UriString, httpClient);
string cachedToken = string.Empty;
string refreshToken = string.Empty;

try
{
    var allPosts = await apiClient.GetAllPostsAsync(null, null);
}
catch (ApiException ex)
{
    // should be unauthorised exception
}

try
{
    var response = await apiClient.RegisterAsync(new UserRegistrationRequest()
    {
        Email = "sob@fu.com",
        Password = "Gfuself123_"
    });
}
catch (ApiException ex)
{

    var responsecode = ex.StatusCode;
    var msg = ex.Message;
}


var loginresponse = await apiClient.LoginAsync(new UserLoginRequest
{
    Email = "sob@fu.com",
    Password = "Gfuself123_"
});

cachedToken = loginresponse.Token;
var handler = new JwtSecurityTokenHandler();
var token = handler.ReadJwtToken(cachedToken);
var expiration = token.ValidTo;
var claims = token.Claims;

refreshToken = loginresponse.RefreshToken;

httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cachedToken);
var allPost = await apiClient.GetAllPostsAsync(null, null);
var createdPost = await apiClient.CreatePostAsync(new CreatePostRequest { Name = "whatever", Tags = new[] { "tag1", "tag2" } });

var retrievedPost = await apiClient.GetPostAsync(createdPost.Data.Id);
var updatedPost = await apiClient.UpdatePostAsync(createdPost.Data.Id, new UpdatePostRequest
{
    Name = "whatever2",
    Tags = new[] { "tag1", "tag3" }
});

// wait till token has expired
await Task.Delay(30000);
var refrequest = new RefreshTokenRequest() { Token = cachedToken, RefreshToken = refreshToken };

var policy = Polly.Policy
    .Handle<ApiException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized or (int)HttpStatusCode.Forbidden)
    .RetryAsync(
    retryCount: 1,
    onRetryAsync: async (_, _) =>
    {
        Console.WriteLine($"refresh the token {cachedToken}");
        AuthSuccessResponse response = await
         apiClient.RefreshAsync(refrequest);
        cachedToken = response.Token;
        refreshToken = response.RefreshToken;
        Console.WriteLine($"new token {cachedToken}");
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cachedToken);

    }
    );

retrievedPost = await policy.ExecuteAsync<PostResponseResponse>(async() => await apiClient.GetPostAsync(createdPost.Data.Id));

await apiClient.DeletePostAsync(createdPost.Data.Id);
allPost = await apiClient.GetAllPostsAsync(null, null);
Console.ReadLine();
