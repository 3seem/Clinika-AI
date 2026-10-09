using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Batch6_Pharmacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pharmacies",
                columns: table => new
                {
                    pharmacy_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pharmacy_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    commercial_registration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pharmacies", x => x.pharmacy_id);
                    table.ForeignKey(
                        name: "fk_pharmacies_users_pharmacy_id",
                        column: x => x.pharmacy_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prescriptions",
                columns: table => new
                {
                    prescription_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    encounter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    doctor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    qr_code_signature = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    raw_ocr_image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    issued_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prescriptions", x => x.prescription_id);
                    table.CheckConstraint("CK_prescriptions_status", "[status] IN ('issued','partially_dispensed','dispensed','cancelled')");
                    table.ForeignKey(
                        name: "fk_prescriptions_doctors_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prescriptions_emr_encounters_encounter_id",
                        column: x => x.encounter_id,
                        principalTable: "emr_encounters",
                        principalColumn: "encounter_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prescriptions_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pharmacy_inventory",
                columns: table => new
                {
                    inventory_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pharmacy_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    drug_id = table.Column<int>(type: "int", nullable: false),
                    batch_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    stock_quantity = table.Column<int>(type: "int", nullable: false),
                    price_per_unit = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_quarantined = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pharmacy_inventory", x => x.inventory_id);
                    table.CheckConstraint("CK_Inventory_Stock", "[stock_quantity] >= 0");
                    table.ForeignKey(
                        name: "fk_pharmacy_inventory_drugs_master_drug_id",
                        column: x => x.drug_id,
                        principalTable: "drugs_master",
                        principalColumn: "drug_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pharmacy_inventory_pharmacies_pharmacy_id",
                        column: x => x.pharmacy_id,
                        principalTable: "pharmacies",
                        principalColumn: "pharmacy_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pharmacy_orders",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pharmacy_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    delivery_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    order_status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    driver_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    driver_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pharmacy_orders", x => x.order_id);
                    table.CheckConstraint("CK_pharmacy_orders_payment", "[payment_method] IN ('cash_on_delivery','card_online')");
                    table.CheckConstraint("CK_pharmacy_orders_status", "[order_status] IN ('received','packed','out_for_delivery','dispensed','cancelled')");
                    table.ForeignKey(
                        name: "fk_pharmacy_orders_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pharmacy_orders_pharmacies_pharmacy_id",
                        column: x => x.pharmacy_id,
                        principalTable: "pharmacies",
                        principalColumn: "pharmacy_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pharmacy_staff_members",
                columns: table => new
                {
                    staff_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pharmacy_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pharmacy_staff_members", x => x.staff_user_id);
                    table.ForeignKey(
                        name: "fk_pharmacy_staff_members_pharmacies_pharmacy_id",
                        column: x => x.pharmacy_id,
                        principalTable: "pharmacies",
                        principalColumn: "pharmacy_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pharmacy_staff_members_users_staff_user_id",
                        column: x => x.staff_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prescription_items",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    prescription_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    drug_id = table.Column<int>(type: "int", nullable: false),
                    dosage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    duration_days = table.Column<int>(type: "int", nullable: false),
                    quantity_prescribed = table.Column<int>(type: "int", nullable: false),
                    instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_dispensed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prescription_items", x => x.item_id);
                    table.CheckConstraint("CK_PrescriptionItem_Qty", "[quantity_prescribed] > 0");
                    table.ForeignKey(
                        name: "fk_prescription_items_drugs_master_drug_id",
                        column: x => x.drug_id,
                        principalTable: "drugs_master",
                        principalColumn: "drug_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prescription_items_prescriptions_prescription_id",
                        column: x => x.prescription_id,
                        principalTable: "prescriptions",
                        principalColumn: "prescription_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pharmacy_order_items",
                columns: table => new
                {
                    order_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    prescription_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inventory_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    quantity_dispensed = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, computedColumnSql: "[quantity_dispensed] * [unit_price]", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pharmacy_order_items", x => x.order_item_id);
                    table.CheckConstraint("CK_PharmacyOrderItem_Qty", "[quantity_dispensed] > 0");
                    table.ForeignKey(
                        name: "fk_pharmacy_order_items_pharmacy_inventory_inventory_id",
                        column: x => x.inventory_id,
                        principalTable: "pharmacy_inventory",
                        principalColumn: "inventory_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pharmacy_order_items_pharmacy_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "pharmacy_orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pharmacy_order_items_prescription_items_prescription_item_id",
                        column: x => x.prescription_item_id,
                        principalTable: "prescription_items",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pharmacies_commercial_registration",
                table: "pharmacies",
                column: "commercial_registration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_inventory_drug_id",
                table: "pharmacy_inventory",
                column: "drug_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_inventory_pharmacy_id",
                table: "pharmacy_inventory",
                column: "pharmacy_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_order_items_inventory_id",
                table: "pharmacy_order_items",
                column: "inventory_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_order_items_order_id",
                table: "pharmacy_order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_order_items_prescription_item_id",
                table: "pharmacy_order_items",
                column: "prescription_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_orders_patient_id",
                table: "pharmacy_orders",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_orders_pharmacy_id",
                table: "pharmacy_orders",
                column: "pharmacy_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_staff_members_pharmacy_id",
                table: "pharmacy_staff_members",
                column: "pharmacy_id");

            migrationBuilder.CreateIndex(
                name: "ix_prescription_items_drug_id",
                table: "prescription_items",
                column: "drug_id");

            migrationBuilder.CreateIndex(
                name: "ix_prescription_items_prescription_id",
                table: "prescription_items",
                column: "prescription_id");

            migrationBuilder.CreateIndex(
                name: "ix_prescriptions_doctor_id",
                table: "prescriptions",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "ix_prescriptions_patient_id",
                table: "prescriptions",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "ix_prescriptions_qr_code_signature",
                table: "prescriptions",
                column: "qr_code_signature",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Prescriptions_Encounter",
                table: "prescriptions",
                column: "encounter_id",
                unique: true,
                filter: "[encounter_id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pharmacy_order_items");

            migrationBuilder.DropTable(
                name: "pharmacy_staff_members");

            migrationBuilder.DropTable(
                name: "pharmacy_inventory");

            migrationBuilder.DropTable(
                name: "pharmacy_orders");

            migrationBuilder.DropTable(
                name: "prescription_items");

            migrationBuilder.DropTable(
                name: "pharmacies");

            migrationBuilder.DropTable(
                name: "prescriptions");
        }
    }
}
