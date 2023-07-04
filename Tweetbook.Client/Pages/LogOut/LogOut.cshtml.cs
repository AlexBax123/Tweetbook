using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tweetbook.Client.Pages.LogOut
{
    public class LogOutModel : PageModel
    {
        public void OnGet()
        {
            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("refreshtoken");
            HttpContext.Session.Remove("UserName");
        }
    }
}
