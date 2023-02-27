using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Requests.Queries;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Helpers
{
    public class PaginationHelpers
    {

       public static PagedResponse<T> CreatePaginationResponse<T>(IUriService uriService, PaginationFilter paginationFilter, List<T> postsResponse)
        {
            var nextPage = paginationFilter.PageNumber >= 1 ? uriService.GetAllPostsUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)) : null;
            var previousPage = paginationFilter.PageNumber - 1 >= 1 ? uriService.GetAllPostsUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)) : null;

            var paginationResponse = new PagedResponse<T>
            {
                Data = postsResponse,
                NextPage = postsResponse.Any() ? nextPage?.ToString() : null,
                PreviousPage = previousPage?.ToString(),
                PageNumber = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize
            };
            return paginationResponse;
        }
    }
}
