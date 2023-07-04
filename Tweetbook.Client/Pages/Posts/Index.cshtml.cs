using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetbook.Client.Services;
using TweetbookApi;

namespace Tweetbook.Razor.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly ITweetbookHelper _tweetbookHelper;

        public IndexModel(ITweetbookHelper tweetbookHelper)
        {
            _tweetbookHelper = tweetbookHelper;
        }

        public IList<PostResponse> Posts { get;set; } = new List<PostResponse>();
        public ICollection<string> Errors { get; private set; } = new List<string>();

        public async Task OnGetAsync()
        {
            var respons = await _tweetbookHelper.GetAllPostsAsync();
            if(respons.Success)
                Posts = respons.PostResponsePagedResponse.Data.ToList();

            Errors = respons.Errors;

        }
    }
}
