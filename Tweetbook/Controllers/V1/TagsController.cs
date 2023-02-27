using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Authorization;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagsController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Tags.GetAll, Name = "GetAllTags")]
        //[Authorize(Policy = AuthorizationPolicies.TagViewer)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<TagResponse>>(await _tagService.GetTagsAsync()));
        }

        [HttpDelete(ApiRoutes.Tags.Delete, Name = "DeleteTag")]
        //[Authorize(Roles ="Admin")]
        [Authorize(Policy = AuthorizationPolicies.MustWorkForCompany)]
        public async Task<IActionResult> Delete([FromRoute] Guid tagId)
        {
            var deleted = await _tagService.DeleteTagAsync(tagId);
            if (deleted)
                return NoContent();
            return NotFound();
        }

    }
}
