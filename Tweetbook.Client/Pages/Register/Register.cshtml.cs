using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetbook.Client.Models;
using Tweetbook.Client.Services;

namespace Tweetbook.Client.Pages.Register
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly ITweetbookHelper _tweetbookHelper;

        public RegisterModel(ILogger<RegisterModel> logger, ITweetbookHelper tweetbookHelper)
        {
            _logger = logger;
            _tweetbookHelper = tweetbookHelper;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public RegisterData Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }
        public ICollection<string> Errors { get; private set; } = new List<string>();

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("refreshtoken");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _tweetbookHelper.Register(new RegisterData() { Email = Input.Email, Password = Input.Password });
                if (result.Success)
                {
                    _logger.LogInformation("User logged in.");
                    Errors = new List<string>();
                    Response.Cookies.Append("UserName", Input.Email);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    Errors = result.Errors;
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
