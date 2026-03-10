using Application.DTOs.Auth;
using Application.Features.Auth.Commands.Login; 
using Application.Features.Auth.Commands.Login.GoogleLogin;
using Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("StrictPolicy")]
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
        [AllowAnonymous] // No necesitas token para logearte
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var command = new LoginCommand(dto.Email, dto.Password);
                var token = await _mediator.Send(command);

                return Ok(new { Token = token, Message = "Login exitoso" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}