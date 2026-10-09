using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities;

public class PaymentTransaction
{
    public Guid TransactionId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid? AppointmentId { get; set; }
    public Guid? LabOrderId { get; set; }
    public Guid? PharmacyOrderId { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal NetPayout { get; set; }
    public string PaymentStatus { get; set; } = "pending";
    public string? GatewayTransactionId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public class AuditLog
{
    public long LogId { get; set; }
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityAffected { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
