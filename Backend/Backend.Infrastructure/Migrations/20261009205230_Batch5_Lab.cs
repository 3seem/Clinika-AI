using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Batch5_Lab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "diagnostic_labs",
                columns: table => new
                {
                    lab_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lab_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    commercial_registry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_diagnostic_labs", x => x.lab_id);
                    table.ForeignKey(
                        name: "fk_diagnostic_labs_users_lab_id",
                        column: x => x.lab_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lab_offered_tests",
                columns: table => new
                {
                    offered_test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lab_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    master_test_id = table.Column<int>(type: "int", nullable: false),
                    custom_price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    preparation_instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    turnaround_time_hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lab_offered_tests", x => x.offered_test_id);
                    table.ForeignKey(
                        name: "fk_lab_offered_tests_diagnostic_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "diagnostic_labs",
                        principalColumn: "lab_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_lab_offered_tests_lab_tests_master_master_test_id",
                        column: x => x.master_test_id,
                        principalTable: "lab_tests_master",
                        principalColumn: "test_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lab_staff_members",
                columns: table => new
                {
                    staff_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lab_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lab_staff_members", x => x.staff_user_id);
                    table.ForeignKey(
                        name: "fk_lab_staff_members_diagnostic_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "diagnostic_labs",
                        principalColumn: "lab_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lab_staff_members_users_staff_user_id",
                        column: x => x.staff_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phlebotomists",
                columns: table => new
                {
                    phlebotomist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lab_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    national_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_available = table.Column<bool>(type: "bit", nullable: false),
                    current_location_lat = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    current_location_lng = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phlebotomists", x => x.phlebotomist_id);
                    table.ForeignKey(
                        name: "fk_phlebotomists_diagnostic_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "diagnostic_labs",
                        principalColumn: "lab_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lab_orders",
                columns: table => new
                {
                    lab_order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    doctor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    lab_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assigned_phlebotomist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_home_collection = table.Column<bool>(type: "bit", nullable: false),
                    collection_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lab_orders", x => x.lab_order_id);
                    table.CheckConstraint("CK_lab_orders_status", "[status] IN ('ordered','sample_collected','processing','under_review','published','cancelled')");
                    table.ForeignKey(
                        name: "fk_lab_orders_diagnostic_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "diagnostic_labs",
                        principalColumn: "lab_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lab_orders_doctors_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lab_orders_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lab_orders_phlebotomist_assigned_phlebotomist_id",
                        column: x => x.assigned_phlebotomist_id,
                        principalTable: "phlebotomists",
                        principalColumn: "phlebotomist_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_radiology_analyses",
                columns: table => new
                {
                    analysis_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lab_order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    imaging_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    original_image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    heatmap_gradcam_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    dicom_study_uid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    detected_condition = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    confidence_score = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: true),
                    risk_level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_verified_by_doctor = table.Column<bool>(type: "bit", nullable: false),
                    doctor_feedback_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reviewed_by_doctor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reviewed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ai_radiology_analyses", x => x.analysis_id);
                    table.CheckConstraint("CK_ai_radiology_risk", "[risk_level] IN ('low','medium','high','critical')");
                    table.CheckConstraint("CK_AIRadiology_Confidence", "[confidence_score] IS NULL OR ([confidence_score] BETWEEN 0.0000 AND 1.0000)");
                    table.ForeignKey(
                        name: "fk_ai_radiology_analyses_doctor_reviewed_by_doctor_id",
                        column: x => x.reviewed_by_doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ai_radiology_analyses_lab_order_lab_order_id",
                        column: x => x.lab_order_id,
                        principalTable: "lab_orders",
                        principalColumn: "lab_order_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ai_radiology_analyses_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lab_order_items",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lab_order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    offered_test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sample_barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    price_at_order = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    result_numeric = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: true),
                    result_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    is_abnormal = table.Column<bool>(type: "bit", nullable: false),
                    pathologist_notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lab_order_items", x => x.item_id);
                    table.ForeignKey(
                        name: "fk_lab_order_items_lab_offered_tests_offered_test_id",
                        column: x => x.offered_test_id,
                        principalTable: "lab_offered_tests",
                        principalColumn: "offered_test_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lab_order_items_lab_orders_lab_order_id",
                        column: x => x.lab_order_id,
                        principalTable: "lab_orders",
                        principalColumn: "lab_order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ai_radiology_analyses_lab_order_id",
                table: "ai_radiology_analyses",
                column: "lab_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_ai_radiology_analyses_patient_id",
                table: "ai_radiology_analyses",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "ix_ai_radiology_analyses_reviewed_by_doctor_id",
                table: "ai_radiology_analyses",
                column: "reviewed_by_doctor_id");

            migrationBuilder.CreateIndex(
                name: "ix_diagnostic_labs_commercial_registry",
                table: "diagnostic_labs",
                column: "commercial_registry",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lab_offered_tests_lab_id",
                table: "lab_offered_tests",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_offered_tests_master_test_id",
                table: "lab_offered_tests",
                column: "master_test_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_order_items_lab_order_id",
                table: "lab_order_items",
                column: "lab_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_order_items_offered_test_id",
                table: "lab_order_items",
                column: "offered_test_id");

            migrationBuilder.CreateIndex(
                name: "UQ_LabOrderItems_Barcode",
                table: "lab_order_items",
                column: "sample_barcode",
                unique: true,
                filter: "[sample_barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_lab_orders_assigned_phlebotomist_id",
                table: "lab_orders",
                column: "assigned_phlebotomist_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_orders_doctor_id",
                table: "lab_orders",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_orders_lab_id",
                table: "lab_orders",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_orders_patient_id",
                table: "lab_orders",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_staff_members_lab_id",
                table: "lab_staff_members",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_phlebotomists_lab_id",
                table: "phlebotomists",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_phlebotomists_national_id",
                table: "phlebotomists",
                column: "national_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_radiology_analyses");

            migrationBuilder.DropTable(
                name: "lab_order_items");

            migrationBuilder.DropTable(
                name: "lab_staff_members");

            migrationBuilder.DropTable(
                name: "lab_offered_tests");

            migrationBuilder.DropTable(
                name: "lab_orders");

            migrationBuilder.DropTable(
                name: "phlebotomists");

            migrationBuilder.DropTable(
                name: "diagnostic_labs");
        }
    }
}
