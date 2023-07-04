using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetbook.Client.Models;
using Tweetbook.Client.Services;

using TweetbookApi;

namespace Tweetbook.Razor.Pages.Posts
{
    public class EditModel : PageModel
    {
        private readonly ITweetbookHelper _tweetbookHelper;

        public EditModel(ITweetbookHelper tweetbookHelper)
        {
            _tweetbookHelper = tweetbookHelper;
        }

        [BindProperty]
        public UpdatePost Post { get; set; } = default!;
        public ICollection<string> Errors { get; private set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respons = await _tweetbookHelper.GetPostAsync(id.Value);
            if (respons.Success)
                Post = new UpdatePost()
                {
                    Name = respons.PostResponseResponse.Data.Name,
                    PostId=respons.PostResponseResponse.Data.Id,
                    Tags = string.Join(" ", respons.PostResponseResponse.Data.Tags.Select(t=>t.Name)),
                    UserId= respons.PostResponseResponse.Data.UserId
                }; ;

            Errors = respons.Errors;

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var respons = await _tweetbookHelper.UpdatePostAsync(Post);
            if (respons.Success)

                return RedirectToPage("Index");
            Errors = respons.Errors;
            return Page();

            return RedirectToPage("./Index");
        }


    }
}
