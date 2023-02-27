using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Extensions;

namespace Tweetbook.Controllers.V1
{
    public class OptionsController : Controller
    {
        private readonly IOptions<MyAwesomeOptions> _options;

        public OptionsController(IOptions<MyAwesomeOptions> options)
        {
            _options = options;
        }

        [HttpGet(ApiRoutes.Options.Get, Name = nameof(Options))]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(MyAwesomeOptions))]
        public IActionResult Options()
        {
            return Ok(_options.Value);
        }
    }
}
