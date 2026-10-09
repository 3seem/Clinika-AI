using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Persistence.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> e)
    {
        e.ToTable("transactions", t =>
        {
            t.HasCheckConstraint("CK_transactions_status", "[payment_status] IN ('pending','successful','failed','refunded')");
            t.HasCheckConstraint("CK_Transaction_SingleTarget",
                "(CASE WHEN [appointment_id] IS NOT NULL THEN 1 ELSE 0 END + " +
                "CASE WHEN [lab_order_id] IS NOT NULL THEN 1 ELSE 0 END + " +
                "CASE WHEN [pharmacy_order_id] IS NOT NULL THEN 1 ELSE 0 END) = 1");
            t.HasCheckConstraint("CK_Transaction_Math", "[net_payout] = [gross_amount] - [platform_fee]");
        });
        e.HasKey(x => x.TransactionId);
        e.Property(x => x.GrossAmount).HasPrecision(10, 2);
        e.Property(x => x.PlatformFee).HasPrecision(10, 2);
        e.Property(x => x.NetPayout).HasPrecision(10, 2);
        e.Property(x => x.PaymentStatus).HasMaxLength(20).IsRequired();
        e.Property(x => x.GatewayTransactionId).HasMaxLength(255);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Appointment>().WithMany().HasForeignKey(x => x.AppointmentId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<LabOrder>().WithMany().HasForeignKey(x => x.LabOrderId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<PharmacyOrder>().WithMany().HasForeignKey(x => x.PharmacyOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> e)
    {
        e.ToTable("audit_logs", t => t.UseSqlOutputClause(false));  // عشان في trigger على الجدول
        e.HasKey(x => x.LogId);
        e.Property(x => x.Action).HasMaxLength(100).IsRequired();
        e.Property(x => x.EntityAffected).HasMaxLength(50).IsRequired();
        e.Property(x => x.EntityId).HasMaxLength(255).IsRequired();
        e.Property(x => x.IpAddress).HasMaxLength(45);
        e.HasIndex(x => x.Timestamp);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
