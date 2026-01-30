using Application.Features.Auth.Commands.Login.GoogleLogin;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.Login; 
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Auth;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new user with Email and Password.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            AuthResponseDto response = await _mediator.Send(command);
            
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Authenticates a user using a Google ID Token.
        /// </summary>
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommand command)
        {
            AuthResponseDto response = await _mediator.Send(command);

            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }

        /// <summary>
        /// Authenticates a user using standard Email and Password.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
           /* LoginCommandDto add the dto attributes when you have the handler of the command */ var response = await _mediator.Send(command);

       //     if (!response.Success)
       //         return Unauthorized(response);

            return Ok(response);
        }
    }
}