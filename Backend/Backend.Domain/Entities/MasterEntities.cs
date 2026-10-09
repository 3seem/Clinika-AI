using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities;

public class MedicalSpecialty
{
    public int SpecialtyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class DrugMaster
{
    public int DrugId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string? Form { get; set; }
    public string? StandardStrength { get; set; }
    public string? AtcCode { get; set; }
    public bool IsActive { get; set; } = true;
}

public class DrugInteraction
{
    public int InteractionId { get; set; }
    public int DrugAId { get; set; }
    public int DrugBId { get; set; }
    public string SeverityLevel { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class LabTestMaster
{
    public int TestId { get; set; }
    public string? LoincCode { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? StandardUnit { get; set; }
    public string? ReferenceRangeMale { get; set; }
    public string? ReferenceRangeFemale { get; set; }
}
