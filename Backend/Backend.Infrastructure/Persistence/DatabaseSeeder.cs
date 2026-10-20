using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        // 1. Ensure core medical specialties exist
        if (!await context.MedicalSpecialties.AnyAsync())
        {
            var specialties = new List<MedicalSpecialty>
            {
                new() { Name = "General Medicine", Description = "Primary care and general health consultations" },
                new() { Name = "Cardiology", Description = "Heart and cardiovascular system diagnostics and care" },
                new() { Name = "Dermatology", Description = "Skin, hair, nail conditions and lesion diagnostics" },
                new() { Name = "Pulmonology / Chest", Description = "Respiratory diseases, chest infections, and lungs" },
                new() { Name = "Radiology", Description = "Diagnostic imaging, X-Ray, CT, and MRI review" },
                new() { Name = "Pediatrics", Description = "Infant, child, and adolescent healthcare" },
                new() { Name = "Orthopedics", Description = "Bones, joints, ligaments, and muscular conditions" },
                new() { Name = "Neurology", Description = "Brain, spinal cord, and nervous system disorders" },
                new() { Name = "Internal Medicine", Description = "Comprehensive adult medicine and chronic disease management" },
                new() { Name = "Ophthalmology", Description = "Eye care, vision, and optical treatments" }
            };
            await context.MedicalSpecialties.AddRangeAsync(specialties);
            await context.SaveChangesAsync();
        }

        // 2. Ensure default Super Admin exists
        if (!await context.Users.AnyAsync(u => u.Role == "super_admin"))
        {
            var adminUser = new User
            {
                UserId = Guid.NewGuid(),
                Email = "admin@hospitalia.ai",
                PhoneNumber = "+201000000000",
                PasswordHash = passwordHasher.HashPassword("Admin@Hospitalia2026!"),
                Role = "super_admin",
                IsActive = true,
                IsVerified = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
