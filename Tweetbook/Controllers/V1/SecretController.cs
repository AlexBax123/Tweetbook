using Microsoft.AspNetCore.Mvc;
using Tweetbook.Filters;

namespace Tweetbook.Controllers.V1
{
    [ApiKeyAuth]
    public class SecretController : Controller
    {
        [HttpGet("secret", Name = nameof(GetSecret))]
        public IActionResult GetSecret()
        {
            return Ok("I've got a secret");
        }
    }
}
