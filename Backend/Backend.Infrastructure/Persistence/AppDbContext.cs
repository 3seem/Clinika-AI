using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<VerificationRequest> VerificationRequests => Set<VerificationRequest>();

    public DbSet<MedicalSpecialty> MedicalSpecialties => Set<MedicalSpecialty>();
    public DbSet<DrugMaster> DrugsMaster => Set<DrugMaster>();
    public DbSet<DrugInteraction> DrugInteractions => Set<DrugInteraction>();
    public DbSet<LabTestMaster> LabTestsMaster => Set<LabTestMaster>();

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();
    public DbSet<DoctorLeave> DoctorLeaves => Set<DoctorLeave>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<PatientAllergy> PatientAllergies => Set<PatientAllergy>();

    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<DoctorReview> DoctorReviews => Set<DoctorReview>();
    public DbSet<EmrEncounter> EmrEncounters => Set<EmrEncounter>();
    public DbSet<AiTriageSession> AiTriageSessions => Set<AiTriageSession>();

    public DbSet<DiagnosticLab> DiagnosticLabs => Set<DiagnosticLab>();
    public DbSet<LabStaffMember> LabStaffMembers => Set<LabStaffMember>();
    public DbSet<Phlebotomist> Phlebotomists => Set<Phlebotomist>();
    public DbSet<LabOfferedTest> LabOfferedTests => Set<LabOfferedTest>();
    public DbSet<LabOrder> LabOrders => Set<LabOrder>();
    public DbSet<LabOrderItem> LabOrderItems => Set<LabOrderItem>();
    public DbSet<AiRadiologyAnalysis> AiRadiologyAnalyses => Set<AiRadiologyAnalysis>();

    public DbSet<Pharmacy> Pharmacies => Set<Pharmacy>();
    public DbSet<PharmacyStaffMember> PharmacyStaffMembers => Set<PharmacyStaffMember>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();
    public DbSet<PharmacyInventory> PharmacyInventory => Set<PharmacyInventory>();
    public DbSet<PharmacyOrder> PharmacyOrders => Set<PharmacyOrder>();
    public DbSet<PharmacyOrderItem> PharmacyOrderItems => Set<PharmacyOrderItem>();

    public DbSet<PaymentTransaction> Transactions => Set<PaymentTransaction>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
