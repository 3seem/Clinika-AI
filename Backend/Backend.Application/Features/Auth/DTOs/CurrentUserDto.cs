namespace Backend.Application.Features.Auth.DTOs;

public class CurrentUserDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public object? Profile { get; set; }
}
