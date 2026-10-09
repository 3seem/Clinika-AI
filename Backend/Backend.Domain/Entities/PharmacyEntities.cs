using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities;

public class Pharmacy
{
    public Guid PharmacyId { get; set; }        // PK وهو نفسه FK على users
    public string PharmacyName { get; set; } = string.Empty;
    public string CommercialRegistration { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
}

public class PharmacyStaffMember
{
    public Guid StaffUserId { get; set; }       // PK وهو نفسه FK على users
    public Guid PharmacyId { get; set; }
    public string? RoleTitle { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class Prescription
{
    public Guid PrescriptionId { get; set; } = Guid.NewGuid();
    public Guid? EncounterId { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public string QrCodeSignature { get; set; } = string.Empty;
    public string? RawOcrImageUrl { get; set; }
    public string Status { get; set; } = "issued";
    public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class PrescriptionItem
{
    public Guid ItemId { get; set; } = Guid.NewGuid();
    public Guid PrescriptionId { get; set; }
    public int DrugId { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int QuantityPrescribed { get; set; } = 1;
    public string? Instructions { get; set; }
    public bool IsDispensed { get; set; }
}

public class PharmacyInventory
{
    public Guid InventoryId { get; set; } = Guid.NewGuid();
    public Guid PharmacyId { get; set; }
    public int DrugId { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public bool IsQuarantined { get; set; }
}

public class PharmacyOrder
{
    public Guid OrderId { get; set; } = Guid.NewGuid();
    public Guid PharmacyId { get; set; }
    public Guid PatientId { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = "received";
    public string? DriverName { get; set; }
    public string? DriverPhone { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class PharmacyOrderItem
{
    public Guid OrderItemId { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Guid PrescriptionItemId { get; set; }
    public Guid? InventoryId { get; set; }
    public int QuantityDispensed { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; private set; }   // عمود محسوب في الداتابيز، متكتبش فيه
}
