using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Persistence.Configurations;

public class PharmacyConfiguration : IEntityTypeConfiguration<Pharmacy>
{
    public void Configure(EntityTypeBuilder<Pharmacy> e)
    {
        e.ToTable("pharmacies");
        e.HasKey(x => x.PharmacyId);
        e.Property(x => x.PharmacyId).ValueGeneratedNever();
        e.HasOne<User>().WithOne().HasForeignKey<Pharmacy>(x => x.PharmacyId).OnDelete(DeleteBehavior.Cascade);
        e.Property(x => x.PharmacyName).HasMaxLength(150).IsRequired();
        e.Property(x => x.CommercialRegistration).HasMaxLength(100).IsRequired();
        e.HasIndex(x => x.CommercialRegistration).IsUnique();
        e.Property(x => x.Address).IsRequired();
        e.Property(x => x.ContactPhone).HasMaxLength(20);
    }
}

public class PharmacyStaffMemberConfiguration : IEntityTypeConfiguration<PharmacyStaffMember>
{
    public void Configure(EntityTypeBuilder<PharmacyStaffMember> e)
    {
        e.ToTable("pharmacy_staff_members");
        e.HasKey(x => x.StaffUserId);
        e.Property(x => x.StaffUserId).ValueGeneratedNever();
        e.HasOne<User>().WithOne().HasForeignKey<PharmacyStaffMember>(x => x.StaffUserId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Pharmacy>().WithMany().HasForeignKey(x => x.PharmacyId).OnDelete(DeleteBehavior.Restrict);
        e.Property(x => x.RoleTitle).HasMaxLength(50);
    }
}

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> e)
    {
        e.ToTable("prescriptions", t => t.HasCheckConstraint("CK_prescriptions_status",
            "[status] IN ('issued','partially_dispensed','dispensed','cancelled')"));
        e.HasKey(x => x.PrescriptionId);
        e.Property(x => x.QrCodeSignature).HasMaxLength(255).IsRequired();
        e.HasIndex(x => x.QrCodeSignature).IsUnique();
        e.Property(x => x.RawOcrImageUrl).HasMaxLength(500);
        e.Property(x => x.Status).HasMaxLength(30).IsRequired();
        e.HasIndex(x => x.EncounterId).IsUnique().HasFilter("[encounter_id] IS NOT NULL")
            .HasDatabaseName("UQ_Prescriptions_Encounter");
        e.HasOne<EmrEncounter>().WithMany().HasForeignKey(x => x.EncounterId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
{
    public void Configure(EntityTypeBuilder<PrescriptionItem> e)
    {
        e.ToTable("prescription_items", t => t.HasCheckConstraint("CK_PrescriptionItem_Qty", "[quantity_prescribed] > 0"));
        e.HasKey(x => x.ItemId);
        e.Property(x => x.Dosage).HasMaxLength(50).IsRequired();
        e.Property(x => x.Frequency).HasMaxLength(50).IsRequired();
        e.Property(x => x.CustomDrugName).HasMaxLength(150);
        e.HasOne<Prescription>().WithMany().HasForeignKey(x => x.PrescriptionId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<DrugMaster>().WithMany().HasForeignKey(x => x.DrugId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PharmacyInventoryConfiguration : IEntityTypeConfiguration<PharmacyInventory>
{
    public void Configure(EntityTypeBuilder<PharmacyInventory> e)
    {
        e.ToTable("pharmacy_inventory", t => t.HasCheckConstraint("CK_Inventory_Stock", "[stock_quantity] >= 0"));
        e.HasKey(x => x.InventoryId);
        e.Property(x => x.BatchNumber).HasMaxLength(100).IsRequired();
        e.Property(x => x.PricePerUnit).HasPrecision(10, 2);
        e.HasOne<Pharmacy>().WithMany().HasForeignKey(x => x.PharmacyId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<DrugMaster>().WithMany().HasForeignKey(x => x.DrugId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PharmacyOrderConfiguration : IEntityTypeConfiguration<PharmacyOrder>
{
    public void Configure(EntityTypeBuilder<PharmacyOrder> e)
    {
        e.ToTable("pharmacy_orders", t =>
        {
            t.HasCheckConstraint("CK_pharmacy_orders_payment", "[payment_method] IN ('cash_on_delivery','card_online')");
            t.HasCheckConstraint("CK_pharmacy_orders_status",
                "[order_status] IN ('received','packed','out_for_delivery','dispensed','cancelled')");
        });
        e.HasKey(x => x.OrderId);
        e.Property(x => x.DeliveryAddress).IsRequired();
        e.Property(x => x.TotalAmount).HasPrecision(10, 2);
        e.Property(x => x.PaymentMethod).HasMaxLength(20).IsRequired();
        e.Property(x => x.OrderStatus).HasMaxLength(30).IsRequired();
        e.Property(x => x.DriverName).HasMaxLength(100);
        e.Property(x => x.DriverPhone).HasMaxLength(20);
        e.HasOne<Pharmacy>().WithMany().HasForeignKey(x => x.PharmacyId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PharmacyOrderItemConfiguration : IEntityTypeConfiguration<PharmacyOrderItem>
{
    public void Configure(EntityTypeBuilder<PharmacyOrderItem> e)
    {
        e.ToTable("pharmacy_order_items", t => t.HasCheckConstraint("CK_PharmacyOrderItem_Qty", "[quantity_dispensed] > 0"));
        e.HasKey(x => x.OrderItemId);
        e.Property(x => x.UnitPrice).HasPrecision(10, 2);
        e.Property(x => x.Subtotal).HasPrecision(10, 2)
            .HasComputedColumnSql("[quantity_dispensed] * [unit_price]", stored: true);
        e.HasOne<PharmacyOrder>().WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<PrescriptionItem>().WithMany().HasForeignKey(x => x.PrescriptionItemId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<PharmacyInventory>().WithMany().HasForeignKey(x => x.InventoryId).OnDelete(DeleteBehavior.Restrict);
    }
}
