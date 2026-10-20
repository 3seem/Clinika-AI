using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<VerificationRequest> VerificationRequests { get; }

    DbSet<MedicalSpecialty> MedicalSpecialties { get; }
    DbSet<DrugMaster> DrugsMaster { get; }
    DbSet<DrugInteraction> DrugInteractions { get; }
    DbSet<LabTestMaster> LabTestsMaster { get; }

    DbSet<Doctor> Doctors { get; }
    DbSet<DoctorSchedule> DoctorSchedules { get; }
    DbSet<DoctorLeave> DoctorLeaves { get; }
    DbSet<Patient> Patients { get; }
    DbSet<PatientAllergy> PatientAllergies { get; }

    DbSet<Appointment> Appointments { get; }
    DbSet<DoctorReview> DoctorReviews { get; }
    DbSet<EmrEncounter> EmrEncounters { get; }
    DbSet<AiTriageSession> AiTriageSessions { get; }

    DbSet<DiagnosticLab> DiagnosticLabs { get; }
    DbSet<LabStaffMember> LabStaffMembers { get; }
    DbSet<Phlebotomist> Phlebotomists { get; }
    DbSet<LabOfferedTest> LabOfferedTests { get; }
    DbSet<LabOrder> LabOrders { get; }
    DbSet<LabOrderItem> LabOrderItems { get; }
    DbSet<AiRadiologyAnalysis> AiRadiologyAnalyses { get; }

    DbSet<Pharmacy> Pharmacies { get; }
    DbSet<PharmacyStaffMember> PharmacyStaffMembers { get; }
    DbSet<Prescription> Prescriptions { get; }
    DbSet<PrescriptionItem> PrescriptionItems { get; }
    DbSet<PharmacyInventory> PharmacyInventory { get; }
    DbSet<PharmacyOrder> PharmacyOrders { get; }
    DbSet<PharmacyOrderItem> PharmacyOrderItems { get; }

    DbSet<PaymentTransaction> Transactions { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
