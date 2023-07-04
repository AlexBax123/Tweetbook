

using Polly;
using System.Net;
using System.Text.RegularExpressions;
using Tweetbook.Client.Models;
using TweetbookApi;

namespace Tweetbook.Client.Services
{
    public interface ITweetbookHelper
    {
        Task<CreatePostResponseData> CreatePostAsync(CreatePost createPost);
        Task<LoginResponseData> Login(LoginData loginData);
        Task<RegisterResponseData> Register(RegisterData loginData);
        Task<AllPostsResponseData> GetAllPostsAsync();
        Task<PostResponseData> GetPostAsync(Guid id);
        Task<DeleteResponseData> DeletePostAsync(Guid id);
        Task<UpdatePostResponseData> UpdatePostAsync(UpdatePost updatePost);
    }


    public class TweetbookHelper : ITweetbookHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public TweetbookHelper(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<AllPostsResponseData> GetAllPostsAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var token = session.GetString("token");
            var refreshtoken = session.GetString("refreshtoken");
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new AllPostsResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var apiClient = new TweetbookApi.TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);

            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            var policy = Polly.Policy
                .Handle<ApiException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized or (int)HttpStatusCode.Forbidden)
                .RetryAsync(
                retryCount: 1,
                onRetryAsync: async (_, _) =>
                {
                    Console.WriteLine($"refresh the token {token}");
                    AuthSuccessResponse response = await
                     apiClient.RefreshAsync(refrequest);
                    token = response.Token;
                    refreshtoken = response.RefreshToken;
                    session.SetString("token", token);
                    session.SetString("refreshtoken", refreshtoken);
                    Console.WriteLine($"new token {token}");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                }
                );

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

        public async Task<CreatePostResponseData> CreatePostAsync(CreatePost createPost)
        {
            Regex regex = new Regex("[^a-z^A-Z]");
            var tags = regex.Split(createPost.Tags);
            var createPostRequest = new CreatePostRequest
            {
                Name = createPost.Name,
                Tags = tags
            };

            var session = _httpContextAccessor.HttpContext.Session;
            var token = session.GetString("token");
            var refreshtoken = session.GetString("refreshtoken");
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new CreatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }


            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var apiClient = new TweetbookApi.TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);


            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            var policy = Polly.Policy
                .Handle<ApiException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized or (int)HttpStatusCode.Forbidden)
                .RetryAsync(
                retryCount: 1,
                onRetryAsync: async (_, _) =>
                {
                    Console.WriteLine($"refresh the token {token}");
                    AuthSuccessResponse response = await
                     apiClient.RefreshAsync(refrequest);
                    token = response.Token;
                    refreshtoken = response.RefreshToken;
                    session.SetString("token", token);
                    session.SetString("refreshtoken", refreshtoken);
                    Console.WriteLine($"new token {token}");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                }
                );

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

            var session = _httpContextAccessor.HttpContext.Session;
            var token = session.GetString("token");
            var refreshtoken = session.GetString("refreshtoken");
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new UpdatePostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var apiClient = new TweetbookApi.TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);


            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            var policy = Polly.Policy
                .Handle<ApiException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized or (int)HttpStatusCode.Forbidden)
                .RetryAsync(
                retryCount: 1,
                onRetryAsync: async (_, _) =>
                {
                    Console.WriteLine($"refresh the token {token}");
                    AuthSuccessResponse response = await
                     apiClient.RefreshAsync(refrequest);
                    token = response.Token;
                    refreshtoken = response.RefreshToken;
                    session.SetString("token", token);
                    session.SetString("refreshtoken", refreshtoken);
                    Console.WriteLine($"new token {token}");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                }
                );

            try
            {
                var retrievedPost = await policy.ExecuteAsync(async () => await apiClient.UpdatePostAsync(updatePost.PostId, updatePostRequest));
                return new UpdatePostResponseData()
                {
                    Success = true
                };
            }
            catch   (ApiException<ErrorResponse> ex)
            {
                return new UpdatePostResponseData()
                {
                    Success = false,
                    Errors =  ex.Result.Errors.Select(e=>e.Message).ToList() 
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
            var apiClient = new TweetbookApi.TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);
            try
            {
                    var result = await apiClient.LoginAsync(new UserLoginRequest() { Email = loginData.Email, Password = loginData.Password });
                    var loginResponseData = new LoginResponseData()
                    {
                        Success = true
                    };
                    var session = _httpContextAccessor.HttpContext.Session;
                    session.SetString("token", result.Token);
                    session.SetString("refreshtoken", result.RefreshToken);
                    session.SetString("UserName", loginData.Email);

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
            var apiClient = new TweetbookApi.TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);
            try
            {
                    var result = await apiClient.RegisterAsync(new UserRegistrationRequest() { Email = registerData.Email, Password = registerData.Password });
                    var registerResponseData = new RegisterResponseData()
                    {
                        Success = true
                    };
                    var session = _httpContextAccessor.HttpContext.Session;
                    session.SetString("token", result.Token);
                    session.SetString("refreshtoken", result.RefreshToken);
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
            var session = _httpContextAccessor.HttpContext.Session;
            var token = session.GetString("token");
            var refreshtoken = session.GetString("refreshtoken");
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new PostResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var apiClient = new TweetbookApi.TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);

            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            var policy = Polly.Policy
                .Handle<ApiException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized or (int)HttpStatusCode.Forbidden)
                .RetryAsync(
                retryCount: 1,
                onRetryAsync: async (_, _) =>
                {
                    Console.WriteLine($"refresh the token {token}");
                    AuthSuccessResponse response = await
                     apiClient.RefreshAsync(refrequest);
                    token = response.Token;
                    refreshtoken = response.RefreshToken;
                    session.SetString("token", token);
                    session.SetString("refreshtoken", refreshtoken);
                    Console.WriteLine($"new token {token}");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                }
                );

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
            var session = _httpContextAccessor.HttpContext.Session;
            var token = session.GetString("token");
            var refreshtoken = session.GetString("refreshtoken");
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshtoken))
            {
                return new DeleteResponseData()
                {
                    Success = false,
                    Errors = new List<string>() { "You are not logged In!" }
                };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var apiClient = new TweetbookApi.TweetbookService(_httpClient.BaseAddress.ToString(), _httpClient);

            var refrequest = new RefreshTokenRequest() { Token = token, RefreshToken = refreshtoken };

            var policy = Polly.Policy
                .Handle<ApiException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized or (int)HttpStatusCode.Forbidden)
                .RetryAsync(
                retryCount: 1,
                onRetryAsync: async (_, _) =>
                {
                    Console.WriteLine($"refresh the token {token}");
                    AuthSuccessResponse response = await
                     apiClient.RefreshAsync(refrequest);
                    token = response.Token;
                    refreshtoken = response.RefreshToken;
                    session.SetString("token", token);
                    session.SetString("refreshtoken", refreshtoken);
                    Console.WriteLine($"new token {token}");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                }
                );

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
