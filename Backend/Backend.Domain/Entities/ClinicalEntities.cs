using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities;

public class Appointment
{
    public Guid AppointmentId { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = "booked";
    public string ConsultationType { get; set; } = "clinic";
    public string? ReasonForVisit { get; set; }
    public string? TelehealthRoomId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class DoctorReview
{
    public Guid ReviewId { get; set; } = Guid.NewGuid();
    public Guid AppointmentId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class EmrEncounter
{
    public Guid EncounterId { get; set; } = Guid.NewGuid();
    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public string? Icd10Code { get; set; }
    public string? SubjectiveNotes { get; set; }
    public string? ObjectiveNotes { get; set; }
    public string? AssessmentNotes { get; set; }
    public string? PlanNotes { get; set; }
    public string? AiScribeTranscript { get; set; }
    public DateOnly? FollowUpDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class AiTriageSession
{
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public string SymptomsPrompt { get; set; } = string.Empty;
    public string UrgencyTier { get; set; } = string.Empty;
    public int? SuggestedSpecialtyId { get; set; }
    public string? AgentReasoning { get; set; }      // JSON text
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
