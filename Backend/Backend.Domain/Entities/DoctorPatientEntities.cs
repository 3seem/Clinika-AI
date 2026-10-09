using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities;

public class Doctor
{
    public Guid DoctorId { get; set; }          // PK وهو نفسه FK على users
    public int SpecialtyId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public decimal ConsultationFee { get; set; }
    public int ExperienceYears { get; set; }
    public decimal RatingAverage { get; set; }
    public int TotalReviewsCount { get; set; }
}

public class DoctorSchedule
{
    public Guid ScheduleId { get; set; } = Guid.NewGuid();
    public Guid DoctorId { get; set; }
    public int DayOfWeek { get; set; }          // 0=Sunday, 6=Saturday
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SlotDurationMinutes { get; set; } = 30;
    public bool IsActive { get; set; } = true;
}

public class DoctorLeave
{
    public int LeaveId { get; set; }
    public Guid DoctorId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Reason { get; set; }
}

public class Patient
{
    public Guid PatientId { get; set; }         // PK وهو نفسه FK على users
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string? BloodGroup { get; set; }
    public string? EmergencyContact { get; set; }
}

public class PatientAllergy
{
    public int AllergyId { get; set; }
    public Guid PatientId { get; set; }
    public string Substance { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
}
