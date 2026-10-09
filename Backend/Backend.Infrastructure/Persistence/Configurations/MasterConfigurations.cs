using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Persistence.Configurations;
public class MedicalSpecialtyConfiguration : IEntityTypeConfiguration<MedicalSpecialty>
{
    public void Configure(EntityTypeBuilder<MedicalSpecialty> e)
    {
        e.ToTable("medical_specialties");
        e.HasKey(x => x.SpecialtyId);
        e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        e.HasIndex(x => x.Name).IsUnique();
    }
}

public class DrugMasterConfiguration : IEntityTypeConfiguration<DrugMaster>
{
    public void Configure(EntityTypeBuilder<DrugMaster> e)
    {
        e.ToTable("drugs_master");
        e.HasKey(x => x.DrugId);
        e.Property(x => x.BrandName).HasMaxLength(150).IsRequired();
        e.Property(x => x.GenericName).HasMaxLength(150).IsRequired();
        e.Property(x => x.Form).HasMaxLength(50);
        e.Property(x => x.StandardStrength).HasMaxLength(50);
        e.Property(x => x.AtcCode).HasMaxLength(20);
    }
}

public class DrugInteractionConfiguration : IEntityTypeConfiguration<DrugInteraction>
{
    public void Configure(EntityTypeBuilder<DrugInteraction> e)
    {
        e.ToTable("drug_interactions", t =>
        {
            t.HasCheckConstraint("CK_drug_interactions_severity",
                "[severity_level] IN ('minor','moderate','severe','contraindicated')");
            t.HasCheckConstraint("CK_DrugInteraction_Self", "[drug_a_id] <> [drug_b_id]");
        });
        e.HasKey(x => x.InteractionId);
        e.Property(x => x.SeverityLevel).HasMaxLength(20).IsRequired();
        e.Property(x => x.Description).IsRequired();
        e.HasOne<DrugMaster>().WithMany().HasForeignKey(x => x.DrugAId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<DrugMaster>().WithMany().HasForeignKey(x => x.DrugBId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class LabTestMasterConfiguration : IEntityTypeConfiguration<LabTestMaster>
{
    public void Configure(EntityTypeBuilder<LabTestMaster> e)
    {
        e.ToTable("lab_tests_master");
        e.HasKey(x => x.TestId);
        e.Property(x => x.LoincCode).HasMaxLength(50);
        e.Property(x => x.TestName).HasMaxLength(150).IsRequired();
        e.Property(x => x.Category).HasMaxLength(100);
        e.Property(x => x.StandardUnit).HasMaxLength(50);
        e.Property(x => x.ReferenceRangeMale).HasMaxLength(50);
        e.Property(x => x.ReferenceRangeFemale).HasMaxLength(50);
    }
}