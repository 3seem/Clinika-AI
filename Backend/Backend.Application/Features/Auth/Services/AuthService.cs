using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Settings;
using Backend.Application.Features.Auth.DTOs;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.Application.Features.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IAppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IAppDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken);

        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Your account is currently inactive. Please contact system support.");
        }

        string displayName = user.Email;

        if (user.Role == "patient")
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == user.UserId, cancellationToken);
            if (patient != null)
            {
                displayName = patient.FullName;
            }
        }
        else if (user.Role == "doctor")
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == user.UserId, cancellationToken);
            if (doctor != null && !string.IsNullOrWhiteSpace(doctor.FullName))
            {
                displayName = doctor.FullName;
            }
            else
            {
                displayName = $"Dr. {user.Email.Split('@')[0]}";
            }
        }

        var token = _jwtTokenGenerator.GenerateToken(user, displayName);

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            DisplayName = displayName,
            IsVerified = user.IsVerified,
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryDays)
        };
    }

    public async Task<AuthResponseDto> RegisterPatientAsync(RegisterPatientRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var normalizedPhone = request.PhoneNumber.Trim();

        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("A user with this email address already exists.");
        }

        if (await _context.Users.AnyAsync(u => u.PhoneNumber == normalizedPhone, cancellationToken))
        {
            throw new InvalidOperationException("A user with this phone number already exists.");
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = normalizedEmail,
            PhoneNumber = normalizedPhone,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = "patient",
            IsActive = true,
            IsVerified = true, // Patients are auto-verified upon registration
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var patient = new Patient
        {
            PatientId = user.UserId,
            FullName = request.FullName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender.Trim().ToLowerInvariant(),
            BloodGroup = request.BloodGroup?.Trim(),
            EmergencyContact = request.EmergencyContact?.Trim()
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.Patients.AddAsync(patient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user, patient.FullName);

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            DisplayName = patient.FullName,
            IsVerified = user.IsVerified,
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryDays)
        };
    }

    public async Task<AuthResponseDto> RegisterDoctorAsync(RegisterDoctorRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var normalizedPhone = request.PhoneNumber.Trim();
        var license = request.LicenseNumber.Trim();

        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("A user with this email address already exists.");
        }

        if (await _context.Users.AnyAsync(u => u.PhoneNumber == normalizedPhone, cancellationToken))
        {
            throw new InvalidOperationException("A user with this phone number already exists.");
        }

        if (await _context.Doctors.AnyAsync(d => d.LicenseNumber == license, cancellationToken))
        {
            throw new InvalidOperationException("A doctor with this license number already exists.");
        }

        // Verify specialty exists or fallback
        var specialtyExists = await _context.MedicalSpecialties
            .AnyAsync(s => s.SpecialtyId == request.SpecialtyId, cancellationToken);

        if (!specialtyExists)
        {
            throw new InvalidOperationException($"Specialty with ID {request.SpecialtyId} does not exist.");
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = normalizedEmail,
            PhoneNumber = normalizedPhone,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = "doctor",
            IsActive = true,
            IsVerified = false, // Doctors require admin syndicate/license approval
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var doctor = new Doctor
        {
            DoctorId = user.UserId,
            SpecialtyId = request.SpecialtyId,
            FullName = request.FullName.Trim(),
            Title = string.IsNullOrWhiteSpace(request.Title) ? "Specialist" : request.Title.Trim(),
            LicenseNumber = license,
            ClinicName = request.ClinicName?.Trim(),
            ClinicAddress = request.ClinicAddress?.Trim(),
            ClinicCity = request.ClinicCity?.Trim(),
            ClinicLat = request.ClinicLat,
            ClinicLng = request.ClinicLng,
            ProfilePictureUrl = request.ProfilePictureUrl?.Trim(),
            Bio = request.Bio?.Trim(),
            ConsultationFee = request.ConsultationFee,
            ExperienceYears = request.ExperienceYears,
            RatingAverage = 0.00m,
            TotalReviewsCount = 0
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.Doctors.AddAsync(doctor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        string displayName = doctor.FullName;
        var token = _jwtTokenGenerator.GenerateToken(user, displayName);

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            DisplayName = displayName,
            IsVerified = user.IsVerified,
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryDays)
        };
    }

    public async Task<CurrentUserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        object? profile = null;

        if (user.Role == "patient")
        {
            profile = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == user.UserId, cancellationToken);
        }
        else if (user.Role == "doctor")
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == user.UserId, cancellationToken);

            if (doctor != null)
            {
                var specialty = await _context.MedicalSpecialties
                    .FirstOrDefaultAsync(s => s.SpecialtyId == doctor.SpecialtyId, cancellationToken);

                profile = new
                {
                    doctor.DoctorId,
                    doctor.SpecialtyId,
                    SpecialtyName = specialty?.Name,
                    doctor.FullName,
                    doctor.Title,
                    doctor.LicenseNumber,
                    doctor.ClinicName,
                    doctor.ClinicAddress,
                    doctor.ClinicCity,
                    doctor.ClinicLat,
                    doctor.ClinicLng,
                    doctor.ProfilePictureUrl,
                    doctor.Bio,
                    doctor.ConsultationFee,
                    doctor.ExperienceYears,
                    doctor.RatingAverage,
                    doctor.TotalReviewsCount
                };
            }
        }

        return new CurrentUserDto
        {
            UserId = user.UserId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            IsActive = user.IsActive,
            IsVerified = user.IsVerified,
            CreatedAt = user.CreatedAt,
            Profile = profile
        };
    }
}
