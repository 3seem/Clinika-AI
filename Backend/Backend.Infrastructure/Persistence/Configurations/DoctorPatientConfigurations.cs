using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> e)
    {
        e.ToTable("doctors");
        e.HasKey(x => x.DoctorId);
        e.Property(x => x.DoctorId).ValueGeneratedNever();
        e.HasOne<User>().WithOne().HasForeignKey<Doctor>(x => x.DoctorId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<MedicalSpecialty>().WithMany().HasForeignKey(x => x.SpecialtyId).OnDelete(DeleteBehavior.Restrict);
        e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
        e.Property(x => x.Title).HasMaxLength(50).IsRequired().HasDefaultValue("Specialist");
        e.Property(x => x.LicenseNumber).HasMaxLength(100).IsRequired();
        e.HasIndex(x => x.LicenseNumber).IsUnique();
        e.Property(x => x.ClinicName).HasMaxLength(150);
        e.Property(x => x.ClinicCity).HasMaxLength(50);
        e.Property(x => x.ClinicLat).HasPrecision(9, 6);
        e.Property(x => x.ClinicLng).HasPrecision(9, 6);
        e.Property(x => x.ProfilePictureUrl).HasMaxLength(500);
        e.Property(x => x.ConsultationFee).HasPrecision(10, 2);
        e.Property(x => x.RatingAverage).HasPrecision(3, 2);
    }
}

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> e)
    {
        e.ToTable("doctor_schedules", t =>
        {
            t.HasCheckConstraint("CK_DocSchedule_Day", "[day_of_week] BETWEEN 0 AND 6");
            t.HasCheckConstraint("CK_DocSchedule_Time", "[end_time] > [start_time]");
            t.HasCheckConstraint("CK_DocSchedule_Type", "[consultation_type] IN ('clinic', 'telehealth', 'both')");
        });
        e.HasKey(x => x.ScheduleId);
        e.Property(x => x.ConsultationType).HasMaxLength(20).IsRequired().HasDefaultValue("both");
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class DoctorLeaveConfiguration : IEntityTypeConfiguration<DoctorLeave>
{
    public void Configure(EntityTypeBuilder<DoctorLeave> e)
    {
        e.ToTable("doctor_leaves", t => t.HasCheckConstraint("CK_DocLeave_Date", "[end_date] >= [start_date]"));
        e.HasKey(x => x.LeaveId);
        e.Property(x => x.Reason).HasMaxLength(255);
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> e)
    {
        e.ToTable("patients", t => t.HasCheckConstraint("CK_patients_gender", "[gender] IN ('male','female')"));
        e.HasKey(x => x.PatientId);
        e.Property(x => x.PatientId).ValueGeneratedNever();
        e.HasOne<User>().WithOne().HasForeignKey<Patient>(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
        e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
        e.Property(x => x.Gender).HasMaxLength(10).IsRequired();
        e.Property(x => x.BloodGroup).HasMaxLength(5);
        e.Property(x => x.EmergencyContact).HasMaxLength(20);
    }
}

public class PatientAllergyConfiguration : IEntityTypeConfiguration<PatientAllergy>
{
    public void Configure(EntityTypeBuilder<PatientAllergy> e)
    {
        e.ToTable("patient_allergies", t => t.HasCheckConstraint("CK_patient_allergies_severity",
            "[severity] IN ('mild','moderate','severe')"));
        e.HasKey(x => x.AllergyId);
        e.Property(x => x.Substance).HasMaxLength(100).IsRequired();
        e.Property(x => x.Severity).HasMaxLength(20).IsRequired();
        e.HasIndex(x => new { x.PatientId, x.Substance }).IsUnique();
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
    }
}
