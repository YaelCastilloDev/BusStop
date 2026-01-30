using Domain.Entities;
using System;

namespace Infraestructur.Identity.Models;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiresOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }

    // Navigation property
    public User User { get; set; } = null!;

    // --- Logic Functions ---

    // A token is expired if the current time is greater than or equal to the expiry date
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;

    // A token is active if it hasn't been revoked and it isn't expired
    public bool IsActive => RevokedOn == null && !IsExpired;
}