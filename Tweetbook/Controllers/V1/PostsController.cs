using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Tweetbook.Cache;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Requests.Queries;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Helpers;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostsController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }


        /// <summary>
        /// returns all the Posts with tags
        /// </summary>
        /// <response code="200">returns all the Posts with tags</response>
        [HttpGet(ApiRoutes.Posts.GetAll, Name = "GetAllPosts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<PostResponse>))]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var posts = await _postService.GetPostsAsync(paginationFilter);
            var postsResponse = _mapper.Map<List<PostResponse>>(posts);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<PostResponse>(postsResponse));
            }
            var paginationResponse = PaginationHelpers.CreatePaginationResponse(_uriService, paginationFilter, postsResponse);

            var response = base.Ok(paginationResponse);
            return response ;
        }

        [HttpPut(ApiRoutes.Posts.Update, Name = "UpdatePost")]
        //[Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<PostResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest updatePostRequest)
        {
            var userOwnsPost = await _postService.UserOwnsPost(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "You don't own this post" } } });
            }
            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = updatePostRequest.Name;
            post.Tags = updatePostRequest.Tags.Select(t => new Tag { Name = t }).ToList();
            var updated = await _postService.UpdatePostAsync(post);
            if (updated)
                return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete, Name = "DeletePost")]
        //[Authorize(Roles ="Admin")]
        //[Authorize(Policy = AuthorizationPolicies.MustWorkForCompany)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPost(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You don't own this post" });
            }
            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();
            return NotFound();
        }

        [Cached(600)]
        [HttpGet(ApiRoutes.Posts.Get, Name = "GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<PostResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();
            return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpPost(ApiRoutes.Posts.Create, Name = "CreatePost")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Response<PostResponse>))]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post { Name = postRequest.Name, UserId = HttpContext.GetUserId(), Tags = postRequest.Tags.Select(t => new Tag { Name = t }).ToList() };
            await _postService.CreatePostAsync(post);
            var locationUrl = _uriService.GetPostUri(post.Id.ToString());
            return Created(locationUrl, new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }
    }
}
