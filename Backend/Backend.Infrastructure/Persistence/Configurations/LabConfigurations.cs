using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Persistence.Configurations;

public class DiagnosticLabConfiguration : IEntityTypeConfiguration<DiagnosticLab>
{
    public void Configure(EntityTypeBuilder<DiagnosticLab> e)
    {
        e.ToTable("diagnostic_labs");
        e.HasKey(x => x.LabId);
        e.Property(x => x.LabId).ValueGeneratedNever();
        e.HasOne<User>().WithOne().HasForeignKey<DiagnosticLab>(x => x.LabId).OnDelete(DeleteBehavior.Cascade);
        e.Property(x => x.LabName).HasMaxLength(150).IsRequired();
        e.Property(x => x.CommercialRegistry).HasMaxLength(100).IsRequired();
        e.HasIndex(x => x.CommercialRegistry).IsUnique();
        e.Property(x => x.Address).IsRequired();
        e.Property(x => x.ContactPhone).HasMaxLength(20);
    }
}

public class LabStaffMemberConfiguration : IEntityTypeConfiguration<LabStaffMember>
{
    public void Configure(EntityTypeBuilder<LabStaffMember> e)
    {
        e.ToTable("lab_staff_members");
        e.HasKey(x => x.StaffUserId);
        e.Property(x => x.StaffUserId).ValueGeneratedNever();
        e.HasOne<User>().WithOne().HasForeignKey<LabStaffMember>(x => x.StaffUserId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<DiagnosticLab>().WithMany().HasForeignKey(x => x.LabId).OnDelete(DeleteBehavior.Restrict);
        e.Property(x => x.RoleTitle).HasMaxLength(50);
    }
}

public class PhlebotomistConfiguration : IEntityTypeConfiguration<Phlebotomist>
{
    public void Configure(EntityTypeBuilder<Phlebotomist> e)
    {
        e.ToTable("phlebotomists");
        e.HasKey(x => x.PhlebotomistId);
        e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
        e.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
        e.Property(x => x.NationalId).HasMaxLength(50).IsRequired();
        e.HasIndex(x => x.NationalId).IsUnique();
        e.Property(x => x.CurrentLocationLat).HasPrecision(9, 6);
        e.Property(x => x.CurrentLocationLng).HasPrecision(9, 6);
        e.HasOne<DiagnosticLab>().WithMany().HasForeignKey(x => x.LabId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class LabOfferedTestConfiguration : IEntityTypeConfiguration<LabOfferedTest>
{
    public void Configure(EntityTypeBuilder<LabOfferedTest> e)
    {
        e.ToTable("lab_offered_tests");
        e.HasKey(x => x.OfferedTestId);
        e.Property(x => x.CustomPrice).HasPrecision(10, 2);
        e.HasOne<DiagnosticLab>().WithMany().HasForeignKey(x => x.LabId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<LabTestMaster>().WithMany().HasForeignKey(x => x.MasterTestId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class LabOrderConfiguration : IEntityTypeConfiguration<LabOrder>
{
    public void Configure(EntityTypeBuilder<LabOrder> e)
    {
        e.ToTable("lab_orders", t => t.HasCheckConstraint("CK_lab_orders_status",
            "[status] IN ('ordered','sample_collected','processing','under_review','published','cancelled')"));
        e.HasKey(x => x.LabOrderId);
        e.Property(x => x.Status).HasMaxLength(30).IsRequired();
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<DiagnosticLab>().WithMany().HasForeignKey(x => x.LabId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Phlebotomist>().WithMany().HasForeignKey(x => x.AssignedPhlebotomistId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class LabOrderItemConfiguration : IEntityTypeConfiguration<LabOrderItem>
{
    public void Configure(EntityTypeBuilder<LabOrderItem> e)
    {
        e.ToTable("lab_order_items");
        e.HasKey(x => x.ItemId);
        e.Property(x => x.SampleBarcode).HasMaxLength(100);
        e.HasIndex(x => x.SampleBarcode).IsUnique().HasFilter("[sample_barcode] IS NOT NULL")
            .HasDatabaseName("UQ_LabOrderItems_Barcode");
        e.Property(x => x.PriceAtOrder).HasPrecision(10, 2);
        e.Property(x => x.ResultNumeric).HasPrecision(10, 3);
        e.Property(x => x.Unit).HasMaxLength(30);
        e.HasOne<LabOrder>().WithMany().HasForeignKey(x => x.LabOrderId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<LabOfferedTest>().WithMany().HasForeignKey(x => x.OfferedTestId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AiRadiologyAnalysisConfiguration : IEntityTypeConfiguration<AiRadiologyAnalysis>
{
    public void Configure(EntityTypeBuilder<AiRadiologyAnalysis> e)
    {
        e.ToTable("ai_radiology_analyses", t =>
        {
            t.HasCheckConstraint("CK_ai_radiology_risk", "[risk_level] IN ('low','medium','high','critical')");
            t.HasCheckConstraint("CK_AIRadiology_Confidence",
                "[confidence_score] IS NULL OR ([confidence_score] BETWEEN 0.0000 AND 1.0000)");
        });
        e.HasKey(x => x.AnalysisId);
        e.Property(x => x.ImagingType).HasMaxLength(50).IsRequired();
        e.Property(x => x.OriginalImageUrl).HasMaxLength(500).IsRequired();
        e.Property(x => x.HeatmapGradcamUrl).HasMaxLength(500);
        e.Property(x => x.DicomStudyUid).HasMaxLength(128);
        e.Property(x => x.DetectedCondition).HasMaxLength(150);
        e.Property(x => x.ConfidenceScore).HasPrecision(5, 4);
        e.Property(x => x.RiskLevel).HasMaxLength(20).IsRequired();
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<LabOrder>().WithMany().HasForeignKey(x => x.LabOrderId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.ReviewedByDoctorId).OnDelete(DeleteBehavior.Restrict);
    }
}
