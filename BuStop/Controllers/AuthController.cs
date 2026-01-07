using Application.Features.Auth.Commands.Login.GoogleLogin;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class AuthController : ControllerBase
    {
        /*
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response); */
        }
    }
}
