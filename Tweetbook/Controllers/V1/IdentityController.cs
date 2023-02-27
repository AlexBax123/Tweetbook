using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Produces(MediaTypeNames.Application.Json)]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register, Name = nameof(Register))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccessResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthFailureResponse))]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailureResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authresponse = await _identityService.RegisterAsync(userRegistrationRequest.Email, userRegistrationRequest.Password);
            if (!authresponse.Success)
            {
                return BadRequest(new AuthFailureResponse
                {
                    Errors = authresponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authresponse.Token,
                RefreshToken = authresponse.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Identity.Login, Name = nameof(Login))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccessResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthFailureResponse))]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var authresponse = await _identityService.LoginAsync(userLoginRequest.Email, userLoginRequest.Password);
            if (!authresponse.Success)
            {
                return BadRequest(new AuthFailureResponse
                {
                    Errors = authresponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authresponse.Token,
                RefreshToken = authresponse.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Identity.Refresh, Name = nameof(Refresh))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthSuccessResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthFailureResponse))]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var authresponse = await _identityService.RefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);
            if (!authresponse.Success)
            {
                return BadRequest(new AuthFailureResponse
                {
                    Errors = authresponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authresponse.Token,
                RefreshToken = authresponse.RefreshToken
            });
        }
    }
}
