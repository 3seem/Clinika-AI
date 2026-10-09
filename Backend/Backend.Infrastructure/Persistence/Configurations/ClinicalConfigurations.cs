using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Persistence.Configurations;
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> e)
    {
        e.ToTable("appointments", t =>
        {
            t.HasCheckConstraint("CK_appointments_status", "[status] IN ('booked','completed','cancelled','no_show')");
            t.HasCheckConstraint("CK_appointments_type", "[consultation_type] IN ('clinic','telehealth')");
            t.HasCheckConstraint("CK_Appointments_Time", "[end_time] > [start_time]");
        });
        e.HasKey(x => x.AppointmentId);
        e.Property(x => x.Status).HasMaxLength(20).IsRequired();
        e.Property(x => x.ConsultationType).HasMaxLength(20).IsRequired();
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);

        // يمنع حجز نفس الميعاد لنفس الدكتور مرتين (إلا لو الحجز الأول اتلغى)
        e.HasIndex(x => new { x.DoctorId, x.AppointmentDate, x.StartTime })
            .IsUnique()
            .HasFilter("[status] <> 'cancelled'")
            .HasDatabaseName("UQ_Doctor_Active_Slot");
    }
}

public class DoctorReviewConfiguration : IEntityTypeConfiguration<DoctorReview>
{
    public void Configure(EntityTypeBuilder<DoctorReview> e)
    {
        e.ToTable("doctor_reviews", t =>
        {
            t.HasCheckConstraint("CK_doctor_reviews_rating", "[rating] BETWEEN 1 AND 5");
            t.UseSqlOutputClause(false);   // عشان في trigger على الجدول ده (الدفعة 7)
        });
        e.HasKey(x => x.ReviewId);
        e.HasIndex(x => x.AppointmentId).IsUnique();
        e.HasOne<Appointment>().WithMany().HasForeignKey(x => x.AppointmentId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class EmrEncounterConfiguration : IEntityTypeConfiguration<EmrEncounter>
{
    public void Configure(EntityTypeBuilder<EmrEncounter> e)
    {
        e.ToTable("emr_encounters");
        e.HasKey(x => x.EncounterId);
        e.Property(x => x.Icd10Code).HasColumnName("icd10_code").HasMaxLength(20);
        e.HasIndex(x => x.AppointmentId).IsUnique();
        e.HasOne<Appointment>().WithMany().HasForeignKey(x => x.AppointmentId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AiTriageSessionConfiguration : IEntityTypeConfiguration<AiTriageSession>
{
    public void Configure(EntityTypeBuilder<AiTriageSession> e)
    {
        e.ToTable("ai_triage_sessions", t =>
        {
            t.HasCheckConstraint("CK_ai_triage_urgency", "[urgency_tier] IN ('emergency','urgent','routine')");
            t.HasCheckConstraint("CK_Triage_JSON", "[agent_reasoning] IS NULL OR ISJSON([agent_reasoning]) = 1");
        });
        e.HasKey(x => x.SessionId);
        e.Property(x => x.SymptomsPrompt).IsRequired();
        e.Property(x => x.UrgencyTier).HasMaxLength(20).IsRequired();
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<MedicalSpecialty>().WithMany().HasForeignKey(x => x.SuggestedSpecialtyId).OnDelete(DeleteBehavior.Restrict);
    }
}
