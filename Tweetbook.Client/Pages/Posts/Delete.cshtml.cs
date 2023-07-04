using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Tweetbook.Client.Services;

using TweetbookApi;

namespace Tweetbook.Razor.Pages.Posts
{
    public class DeleteModel : PageModel
    {
        private readonly ITweetbookHelper _tweetbookHelper;

        public DeleteModel(ITweetbookHelper tweetbookHelper)
        {
            _tweetbookHelper = tweetbookHelper;
        }
        public PostResponse Post { get; set; } = default!;
        public ICollection<string> Errors { get; private set; } = new List<string>();


        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respons = await _tweetbookHelper.GetPostAsync(id.Value);
            if (respons.Success)
                Post = respons.PostResponseResponse.Data;

            Errors = respons.Errors;

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var respons = await _tweetbookHelper.DeletePostAsync(id.Value);
            if (respons.Success)
            {
            return RedirectToPage("./Index");

            }
            Errors = respons.Errors;
            return Page();


        }
    }
}
