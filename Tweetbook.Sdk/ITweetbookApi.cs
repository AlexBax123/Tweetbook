using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;

namespace Tweetbook.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface ITweetbookApi
    {
        // This will automagically be populated by Refit if the property exists
        HttpClient Client { get; }

        [Get("/api/v1/posts")]
        Task<ApiResponse<List<PostResponse>>> GetAllPosts();
        [Get("/api/v1/posts/{postId}")]
        Task<ApiResponse<PostResponse>> GetAsync(Guid postId);
        
        [Post("/api/v1/posts")]
        Task<ApiResponse<PostResponse>> CreateAsync([Body] CreatePostRequest createPostRequest);
        
        [Put("/api/v1/posts/{postId}")]
        Task<ApiResponse<PostResponse>> UpdateAsync(Guid postId, [Body] UpdatePostRequest updatepostRequest);

        [Delete("/api/v1/posts/{postId}")]
        Task<ApiResponse<PostResponse>> DeleteAsync(Guid postId);

    }
}
