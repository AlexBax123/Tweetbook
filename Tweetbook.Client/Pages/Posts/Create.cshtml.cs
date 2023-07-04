using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetbook.Client.Models;
using Tweetbook.Client.Services;

namespace Tweetbook.Client.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly ITweetbookHelper _tweetbookHelper;

        public CreateModel(ITweetbookHelper tweetbookHelper)
        {

            _tweetbookHelper = tweetbookHelper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CreatePost CreatePost { get; set; } = default!;
        public ICollection<string> Errors { get; private set; } = new List<string>();

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || CreatePost == null)
            {
                return Page();
            }

            var respons = await _tweetbookHelper.CreatePostAsync(CreatePost);
            if (respons.Success)

                return RedirectToPage("Index");
            Errors = respons.Errors;
            return Page();
        }
    }
}
