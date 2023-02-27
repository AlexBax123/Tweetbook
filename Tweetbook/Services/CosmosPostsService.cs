using Cosmonaut;
using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class CosmosPostsService : IPostService
    {
        private readonly ICosmosStore<CosmosPostDto> _cosmosStore;

        public CosmosPostsService(ICosmosStore<CosmosPostDto> cosmosStore)
        {
            _cosmosStore = cosmosStore;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            var cosmosPostDto = new CosmosPostDto { Id = Guid.NewGuid().ToString(), Name = post.Name, UserId= post.UserId };
            var response = await _cosmosStore.AddAsync(cosmosPostDto);
            if (response.IsSuccess)
                post.Id = new Guid(cosmosPostDto.Id);
            return response.IsSuccess;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var response = await _cosmosStore.RemoveByIdAsync(postId.ToString(), postId.ToString());
            return response.IsSuccess;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await _cosmosStore.FindAsync(postId.ToString(), postId.ToString());
            if (post == null)
                return null;
            return new Post { Id = Guid.Parse(post.Id), Name = post.Name };
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var posts = await _cosmosStore.Query().ToListAsync();
            return posts.Select(p => new Post { Id = Guid.Parse(p.Id), Name = p.Name }).ToList();
        }

        public async Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter = null)
        {
            if (paginationFilter == null)
            {
                return await GetPostsAsync();
            }
            var allPosts = await GetPostsAsync();
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return allPosts.Skip(skip).Take(paginationFilter.PageSize).ToList();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            var cosmosPostDto = new CosmosPostDto { Id = postToUpdate.Id.ToString(), Name = postToUpdate.Name, UserId = postToUpdate.UserId };
            var response = await _cosmosStore.UpdateAsync(cosmosPostDto);
            return response.IsSuccess;
        }

        public async Task<bool> UserOwnsPost(Guid postId, string userId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
                return false;
            if (post.UserId != userId)
                return false;
            return true;
        }
    }
}
