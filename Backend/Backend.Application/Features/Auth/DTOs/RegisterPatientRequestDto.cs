using System.ComponentModel.DataAnnotations;

namespace Backend.Application.Features.Auth.DTOs;

public class RegisterPatientRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [RegularExpression("^(male|female)$", ErrorMessage = "Gender must be 'male' or 'female'")]
    public string Gender { get; set; } = string.Empty;

    public string? BloodGroup { get; set; }

    public string? EmergencyContact { get; set; }
}
