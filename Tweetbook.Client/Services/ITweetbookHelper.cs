using Tweetbook.Client.Models;

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
}
