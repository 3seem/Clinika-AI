using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> e)
    {
        e.ToTable("users", t => t.HasCheckConstraint("CK_users_role",
            "[role] IN ('patient','doctor','lab_staff','pharmacy_staff','super_admin')"));
        e.HasKey(u => u.UserId);
        e.Property(u => u.Email).HasMaxLength(255).IsRequired();
        e.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
        e.Property(u => u.PhoneNumber).HasMaxLength(20).IsRequired();
        e.Property(u => u.Role).HasMaxLength(50).IsRequired();
        e.HasIndex(u => u.Email).IsUnique();
        e.HasIndex(u => u.PhoneNumber).IsUnique();
    }
}

public class VerificationRequestConfiguration : IEntityTypeConfiguration<VerificationRequest>
{
    public void Configure(EntityTypeBuilder<VerificationRequest> e)
    {
        e.ToTable("verification_requests", t => t.HasCheckConstraint("CK_verification_requests_status",
            "[status] IN ('pending','approved','rejected')"));
        e.HasKey(x => x.RequestId);
        e.Property(x => x.DocumentType).HasMaxLength(50).IsRequired();
        e.Property(x => x.DocumentUrl).HasMaxLength(500).IsRequired();
        e.Property(x => x.Status).HasMaxLength(20).IsRequired();
        e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.ReviewedBy).OnDelete(DeleteBehavior.Restrict);
    }
}