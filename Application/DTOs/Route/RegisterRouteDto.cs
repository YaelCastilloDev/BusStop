using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Route
{
    /// <summary>
    /// DTO for registering a new route.
    /// A route is created with its name, description, and an ordered list of stops.
    /// </summary>
    public class RegisterRouteDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public Guid Userid { get; set; }
    }
}