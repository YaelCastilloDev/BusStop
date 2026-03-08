// --- Application/DTOs/Comments/AddCommentDto.cs ---
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Comments
{
    public class AddCommentDto
    {
        [Required]
        public Guid RouteId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;
    }
}