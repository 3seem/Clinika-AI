using System.ComponentModel.DataAnnotations;

namespace Backend.Application.Features.Auth.DTOs;

public class RegisterDoctorRequestDto
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

    [MaxLength(50)]
    public string Title { get; set; } = "Specialist";

    [Required]
    public int SpecialtyId { get; set; }

    [Required]
    [MaxLength(100)]
    public string LicenseNumber { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? ClinicName { get; set; }

    public string? ClinicAddress { get; set; }

    [MaxLength(50)]
    public string? ClinicCity { get; set; }

    public decimal? ClinicLat { get; set; }

    public decimal? ClinicLng { get; set; }

    [MaxLength(500)]
    public string? ProfilePictureUrl { get; set; }

    public string? Bio { get; set; }

    [Required]
    [Range(0, 100000)]
    public decimal ConsultationFee { get; set; }

    [Range(0, 70)]
    public int ExperienceYears { get; set; } = 0;
}
