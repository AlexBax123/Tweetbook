using Microsoft.AspNetCore.Http;
using Polly;
using Polly.Retry;
using System.Net;
using System.Text.RegularExpressions;
using Tweetbook.WebAssembly.Client.Models;
using TweetbookApi;

namespace Tweetbook.WebAssembly.Client.Services
{
    public class TweetbookHelper : ITweetbookHelper
    {
         private readonly HttpClient _httpClient;
        private readonly CookieStorageAccessor _cookieStorageAccessor;

        public TweetbookHelper(
            HttpClient httpClient, CookieStorageAccessor cookieStorageAccessor)//, Blazored.SessionStorage.ISessionStorageService sessionStorage)
        {
            //_httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _cookieStorageAccessor = cookieStorageAccessor;
        }

        public async Task<AllPostsResponseData> GetAllPostsAsync()
        {
            (bool loggedIn, string token) = await LoggedInAsync();
            if (!loggedIn)
            {
                return new AllPostsResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);

            AsyncRetryPolicy policy = GetPolicy();

            try
            {
                var retrievedPost = await policy.ExecuteAsync(async () => await apiClient.GetAllPostsAsync(1, 1000));
                return new AllPostsResponseData()
                {
                    Success = true,
                    PostResponsePagedResponse = retrievedPost
                };
            }
            catch (ApiException ex)
            {
                return new AllPostsResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message },
                    PostResponsePagedResponse = null

                };
            }
            catch (Exception ex)
            {
                return new AllPostsResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message },
                    PostResponsePagedResponse = null
                };
            }

        }

        private async Task SetToken(string token)
        {
            await _cookieStorageAccessor.SetValueAsync<string>("blazor_token", token); 
        }

        private async Task SetRefreshToken(string token)
        {
            await _cookieStorageAccessor.SetValueAsync<string>("blazor_refreshtoken", token);
        }

        private async Task<string> GetToken()
        {
            var blazor_token = await GetCookieValue("blazor_token");
            return blazor_token;
        }

        private async Task<string>GetCookieValue(string key)
        {
            var fullcookie= await _cookieStorageAccessor.GetValueAsync<string>("whatever");
            if (fullcookie == string.Empty)
                return null;   
            var keypairvaluesstrings = fullcookie.Split(';');
            Dictionary<string,string> keyValuePairs = new Dictionary<string,string>();
            foreach (var keypair in keypairvaluesstrings)
            {
                var split = keypair.Split('=');
                keyValuePairs.Add(split[0].Trim(), split[1].Trim());
            }
            keyValuePairs.TryGetValue(key, out string? value);
            return value;
        }


        private async Task<string> GetrefreshToken()
        {
            var blazor_refreshtoken = await GetCookieValue("blazor_refreshtoken");
            return blazor_refreshtoken;
        }
        private async Task<(bool, string)> LoggedInAsync()
        {
            var token = await GetToken();
            var refreshtoken = await GetrefreshToken();
            return (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(refreshtoken), token);
        }

        private AsyncRetryPolicy GetPolicy()
        {
            var policy = Polly.Policy
                .Handle<ApiException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized or (int)HttpStatusCode.Forbidden)
                .RetryAsync(
                retryCount: 1,
                onRetryAsync: async (_, _) =>
                {
                    var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);

                    //var session = _httpContextAccessor.HttpContext.Session;
                    var token = await GetToken();
                    var refreshtoken = await GetrefreshToken();


                    var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };
                    Console.WriteLine($"refresh the token {token}");
                    AuthSuccessResponse response = await
                     apiClient.RefreshAsync(refrequest);
                    token = response.Token;
                    refreshtoken = response.RefreshToken;
                    await SetToken(token);
                    await SetRefreshToken(refreshtoken);
                    Console.WriteLine($"new token {token}");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                }
                );
            return policy;
        }

        public async Task<CreatePostResponseData> CreatePostAsync(CreatePost createPost)
        {
            Regex regex = new Regex("[^a-z^A-Z]");
            var tags = regex.Split(createPost.Tags);
            var createPostRequest = new CreatePostRequest
            {
                Name = createPost.Name,
                Tags = tags
            };

            //var session = _httpContextAccessor.HttpContext.Session;
            var token = await GetToken();
            var refreshtoken = await GetrefreshToken();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new CreatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }


            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);


            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };
            AsyncRetryPolicy policy = GetPolicy();

            try
            {
                var retrievedPost = await policy.ExecuteAsync(async () => await apiClient.CreatePostAsync(createPostRequest));
                return new CreatePostResponseData()
                {
                    Success = true
                };
            }
            catch (ApiException ex)
            {
                return new CreatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
            catch (Exception ex)
            {
                return new CreatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }


        public async Task<UpdatePostResponseData> UpdatePostAsync(UpdatePost updatePost)
        {
            Regex regex = new Regex("[^a-z^A-Z]");
            var tags = regex.Split(updatePost.Tags);
            var updatePostRequest = new UpdatePostRequest
            {
                Name = updatePost.Name,
                Tags = tags

            };

            //var session = _httpContextAccessor.HttpContext.Session;
            var token = await GetToken();
            var refreshtoken = await GetrefreshToken();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new UpdatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);


            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            AsyncRetryPolicy policy = GetPolicy();

            try
            {
                var retrievedPost = await policy.ExecuteAsync(async () => await apiClient.UpdatePostAsync(updatePost.PostId, updatePostRequest));
                return new UpdatePostResponseData()
                {
                    Success = true
                };
            }
            catch (ApiException<ErrorResponse> ex)
            {
                return new UpdatePostResponseData()
                {
                    Success = false,
                    Errors = ex.Result.Errors.Select(e => e.Message).ToList()
                };
            }
            catch (ApiException ex)
            {
                return new UpdatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
            catch (Exception ex)
            {
                return new UpdatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<LoginResponseData> Login(LoginData loginData)
        {
            var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);
            try
            {
                var result = await apiClient.LoginAsync(new UserLoginRequest() { Email = loginData.Email, Password = loginData.Password });
                var loginResponseData = new LoginResponseData()
                {
                    Success = true
                };

                await SetToken(result.Token);
                await SetRefreshToken(result.RefreshToken);
                //await _sessionStorage.SetItemAsync("blazor_UserName", loginData.Email);

                return loginResponseData;
            }
            catch (ApiException<AuthFailureResponse> ex)
            {
                var loginResponseData = new LoginResponseData()
                {
                    Success = false,
                    Errors = ex.Result.Errors
                };
                return loginResponseData;
            }
            catch (ApiException ex)
            {
                var loginResponseData = new LoginResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
                return loginResponseData;
            }
            catch (Exception ex)
            {
                var loginResponseData = new LoginResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
                return loginResponseData;
            }
        }

        public async Task<RegisterResponseData> Register(RegisterData registerData)
        {
            var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);
            try
            {
                var result = await apiClient.RegisterAsync(new UserRegistrationRequest() { Email = registerData.Email, Password = registerData.Password });
                var registerResponseData = new RegisterResponseData()
                {
                    Success = true
                };
               // var session = _httpContextAccessor.HttpContext.Session;
                await SetToken(result.Token);
                await SetRefreshToken(result.RefreshToken);
                return registerResponseData;
            }
            catch (ApiException<AuthFailureResponse> ex)
            {
                var registerResponseData = new RegisterResponseData()
                {
                    Success = false,
                    Errors = ex.Result.Errors
                };
                return registerResponseData;
            }
            catch (ApiException ex)
            {
                var registerResponseData = new RegisterResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
                return registerResponseData;
            }
            catch(HttpRequestException ex)
            {
                var registerResponseData = new RegisterResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
                return registerResponseData;
            }
            catch (Exception ex)
            {
                var registerResponseData = new RegisterResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message }
                };
                return registerResponseData;
            }

        }

        public async Task<PostResponseData> GetPostAsync(Guid id)
        {
            //var session = _httpContextAccessor.HttpContext.Session;
            var token = await GetToken();
            var refreshtoken = await GetrefreshToken();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new PostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);

            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            AsyncRetryPolicy policy = GetPolicy();

            try
            {
                var retrievedPost = await policy.ExecuteAsync(async () => await apiClient.GetPostAsync(id));
                return new PostResponseData()
                {
                    Success = true,
                    PostResponseResponse = retrievedPost
                };
            }
            catch (ApiException ex)
            {
                return new PostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message },
                    PostResponseResponse = null

                };
            }
            catch (Exception ex)
            {
                return new PostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message },
                    PostResponseResponse = null
                };
            }
        }

        public async Task<DeleteResponseData> DeletePostAsync(Guid id)
        {
            // var session = _httpContextAccessor.HttpContext.Session;
            var token = await GetToken();
            var refreshtoken = await GetrefreshToken();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new DeleteResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var apiClient = new TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);

            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            AsyncRetryPolicy policy = GetPolicy();

            try
            {
                await policy.ExecuteAsync(async () => await apiClient.DeletePostAsync(id));
                return new DeleteResponseData()
                {
                    Success = true,
                    Errors = new List<string>(),
                };
            }
            catch (ApiException ex)
            {
                return new DeleteResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message },
                };
            }
            catch (Exception ex)
            {
                return new DeleteResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { ex.Message },
                };
            }
        }
    }
}
