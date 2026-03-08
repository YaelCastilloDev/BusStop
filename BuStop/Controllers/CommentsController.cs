// --- WebApi/Controllers/CommentsController.cs ---
using Application.DTOs.Comments;
using Application.Features.Comments.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // User must be logged in to comment
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDto dto)
        {
            // Extract User ID from the JWT Token securely
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "User ID not found or invalid in token" });
            }

            // Fire the command
            var command = new CreateCommentCommand(dto.RouteId, dto.Text, userId);
            var commentId = await _mediator.Send(command);

            return CreatedAtAction(null, new { CommentId = commentId, Message = "Comment added successfully" });
        }
        // Añade este método dentro de tu WebApi/Controllers/CommentsController.cs

        [HttpPost("{commentId}/react")]
        public async Task<IActionResult> ReactToComment(Guid commentId, [FromBody] ReactToCommentDto dto)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "User ID not found or invalid in token" });
            }

            var command = new ReactToCommentCommand(commentId, userId, dto.Liked);
            var success = await _mediator.Send(command);

            if (success)
            {
                return Ok(new { Message = "Reaction saved successfully" });
            }

            return BadRequest(new { Message = "Failed to save reaction" });
        }
    }


}