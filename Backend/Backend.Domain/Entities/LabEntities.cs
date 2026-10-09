using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities;

public class DiagnosticLab
{
    public Guid LabId { get; set; }             // PK وهو نفسه FK على users
    public string LabName { get; set; } = string.Empty;
    public string CommercialRegistry { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
}

public class LabStaffMember
{
    public Guid StaffUserId { get; set; }       // PK وهو نفسه FK على users
    public Guid LabId { get; set; }
    public string? RoleTitle { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class Phlebotomist
{
    public Guid PhlebotomistId { get; set; } = Guid.NewGuid();
    public Guid LabId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public decimal? CurrentLocationLat { get; set; }
    public decimal? CurrentLocationLng { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    // spatial_location (GEOGRAPHY) اتأجّل
}

public class LabOfferedTest
{
    public Guid OfferedTestId { get; set; } = Guid.NewGuid();
    public Guid LabId { get; set; }
    public int MasterTestId { get; set; }
    public decimal CustomPrice { get; set; }
    public string? PreparationInstructions { get; set; }
    public int TurnaroundTimeHours { get; set; } = 24;
}

public class LabOrder
{
    public Guid LabOrderId { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public Guid LabId { get; set; }
    public Guid? AssignedPhlebotomistId { get; set; }
    public bool IsHomeCollection { get; set; }
    public string? CollectionAddress { get; set; }
    public string Status { get; set; } = "ordered";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class LabOrderItem
{
    public Guid ItemId { get; set; } = Guid.NewGuid();
    public Guid LabOrderId { get; set; }
    public Guid OfferedTestId { get; set; }
    public string? SampleBarcode { get; set; }
    public decimal PriceAtOrder { get; set; }
    public decimal? ResultNumeric { get; set; }
    public string? ResultText { get; set; }
    public string? Unit { get; set; }
    public bool IsAbnormal { get; set; }
    public string? PathologistNotes { get; set; }
}

public class AiRadiologyAnalysis
{
    public Guid AnalysisId { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Guid? LabOrderId { get; set; }
    public string ImagingType { get; set; } = string.Empty;
    public string OriginalImageUrl { get; set; } = string.Empty;
    public string? HeatmapGradcamUrl { get; set; }
    public string? DicomStudyUid { get; set; }
    public string? DetectedCondition { get; set; }
    public decimal? ConfidenceScore { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public bool IsVerifiedByDoctor { get; set; }
    public string? DoctorFeedbackNotes { get; set; }
    public Guid? ReviewedByDoctorId { get; set; }
    public DateTimeOffset? ReviewedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
