using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Batch4_Clinical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ai_triage_sessions",
                columns: table => new
                {
                    session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    symptoms_prompt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    urgency_tier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    suggested_specialty_id = table.Column<int>(type: "int", nullable: true),
                    agent_reasoning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ai_triage_sessions", x => x.session_id);
                    table.CheckConstraint("CK_ai_triage_urgency", "[urgency_tier] IN ('emergency','urgent','routine')");
                    table.CheckConstraint("CK_Triage_JSON", "[agent_reasoning] IS NULL OR ISJSON([agent_reasoning]) = 1");
                    table.ForeignKey(
                        name: "fk_ai_triage_sessions_medical_specialties_suggested_specialty_id",
                        column: x => x.suggested_specialty_id,
                        principalTable: "medical_specialties",
                        principalColumn: "specialty_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ai_triage_sessions_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    appointment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    doctor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    appointment_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    consultation_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_appointments", x => x.appointment_id);
                    table.CheckConstraint("CK_appointments_status", "[status] IN ('booked','completed','cancelled','no_show')");
                    table.CheckConstraint("CK_Appointments_Time", "[end_time] > [start_time]");
                    table.CheckConstraint("CK_appointments_type", "[consultation_type] IN ('clinic','telehealth')");
                    table.ForeignKey(
                        name: "fk_appointments_doctor_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_appointments_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "doctor_reviews",
                columns: table => new
                {
                    review_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    appointment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    doctor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_anonymous = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_doctor_reviews", x => x.review_id);
                    table.CheckConstraint("CK_doctor_reviews_rating", "[rating] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "fk_doctor_reviews_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_doctor_reviews_doctors_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_doctor_reviews_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "emr_encounters",
                columns: table => new
                {
                    encounter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    appointment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    doctor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    icd10_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    subjective_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    objective_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assessment_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    plan_notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ai_scribe_transcript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_emr_encounters", x => x.encounter_id);
                    table.ForeignKey(
                        name: "fk_emr_encounters_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_emr_encounters_doctors_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "doctor_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_emr_encounters_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ai_triage_sessions_patient_id",
                table: "ai_triage_sessions",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "ix_ai_triage_sessions_suggested_specialty_id",
                table: "ai_triage_sessions",
                column: "suggested_specialty_id");

            migrationBuilder.CreateIndex(
                name: "ix_appointments_patient_id",
                table: "appointments",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Doctor_Active_Slot",
                table: "appointments",
                columns: new[] { "doctor_id", "appointment_date", "start_time" },
                unique: true,
                filter: "[status] <> 'cancelled'");

            migrationBuilder.CreateIndex(
                name: "ix_doctor_reviews_appointment_id",
                table: "doctor_reviews",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_doctor_reviews_doctor_id",
                table: "doctor_reviews",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "ix_doctor_reviews_patient_id",
                table: "doctor_reviews",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "ix_emr_encounters_appointment_id",
                table: "emr_encounters",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_emr_encounters_doctor_id",
                table: "emr_encounters",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "ix_emr_encounters_patient_id",
                table: "emr_encounters",
                column: "patient_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_triage_sessions");

            migrationBuilder.DropTable(
                name: "doctor_reviews");

            migrationBuilder.DropTable(
                name: "emr_encounters");

            migrationBuilder.DropTable(
                name: "appointments");
        }
    }
}
