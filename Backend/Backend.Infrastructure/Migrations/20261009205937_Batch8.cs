using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Batch8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ai_radiology_analyses_doctor_reviewed_by_doctor_id",
                table: "ai_radiology_analyses");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_radiology_analyses_lab_order_lab_order_id",
                table: "ai_radiology_analyses");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_radiology_analyses_patient_patient_id",
                table: "ai_radiology_analyses");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_triage_sessions_patient_patient_id",
                table: "ai_triage_sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_appointments_doctor_doctor_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "fk_appointments_patient_patient_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "fk_doctor_reviews_patient_patient_id",
                table: "doctor_reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_emr_encounters_patient_patient_id",
                table: "emr_encounters");

            migrationBuilder.DropForeignKey(
                name: "fk_lab_orders_patient_patient_id",
                table: "lab_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_lab_orders_phlebotomist_assigned_phlebotomist_id",
                table: "lab_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_patient_allergies_patient_patient_id",
                table: "patient_allergies");

            migrationBuilder.AddForeignKey(
                name: "fk_ai_radiology_analyses_doctors_reviewed_by_doctor_id",
                table: "ai_radiology_analyses",
                column: "reviewed_by_doctor_id",
                principalTable: "doctors",
                principalColumn: "doctor_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_radiology_analyses_lab_orders_lab_order_id",
                table: "ai_radiology_analyses",
                column: "lab_order_id",
                principalTable: "lab_orders",
                principalColumn: "lab_order_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_radiology_analyses_patients_patient_id",
                table: "ai_radiology_analyses",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_triage_sessions_patients_patient_id",
                table: "ai_triage_sessions",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_appointments_doctors_doctor_id",
                table: "appointments",
                column: "doctor_id",
                principalTable: "doctors",
                principalColumn: "doctor_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_appointments_patients_patient_id",
                table: "appointments",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_doctor_reviews_patients_patient_id",
                table: "doctor_reviews",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_emr_encounters_patients_patient_id",
                table: "emr_encounters",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lab_orders_patients_patient_id",
                table: "lab_orders",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lab_orders_phlebotomists_assigned_phlebotomist_id",
                table: "lab_orders",
                column: "assigned_phlebotomist_id",
                principalTable: "phlebotomists",
                principalColumn: "phlebotomist_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_patient_allergies_patients_patient_id",
                table: "patient_allergies",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ai_radiology_analyses_doctors_reviewed_by_doctor_id",
                table: "ai_radiology_analyses");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_radiology_analyses_lab_orders_lab_order_id",
                table: "ai_radiology_analyses");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_radiology_analyses_patients_patient_id",
                table: "ai_radiology_analyses");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_triage_sessions_patients_patient_id",
                table: "ai_triage_sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_appointments_doctors_doctor_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "fk_appointments_patients_patient_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "fk_doctor_reviews_patients_patient_id",
                table: "doctor_reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_emr_encounters_patients_patient_id",
                table: "emr_encounters");

            migrationBuilder.DropForeignKey(
                name: "fk_lab_orders_patients_patient_id",
                table: "lab_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_lab_orders_phlebotomists_assigned_phlebotomist_id",
                table: "lab_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_patient_allergies_patients_patient_id",
                table: "patient_allergies");

            migrationBuilder.AddForeignKey(
                name: "fk_ai_radiology_analyses_doctor_reviewed_by_doctor_id",
                table: "ai_radiology_analyses",
                column: "reviewed_by_doctor_id",
                principalTable: "doctors",
                principalColumn: "doctor_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_radiology_analyses_lab_order_lab_order_id",
                table: "ai_radiology_analyses",
                column: "lab_order_id",
                principalTable: "lab_orders",
                principalColumn: "lab_order_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_radiology_analyses_patient_patient_id",
                table: "ai_radiology_analyses",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_triage_sessions_patient_patient_id",
                table: "ai_triage_sessions",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_appointments_doctor_doctor_id",
                table: "appointments",
                column: "doctor_id",
                principalTable: "doctors",
                principalColumn: "doctor_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_appointments_patient_patient_id",
                table: "appointments",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_doctor_reviews_patient_patient_id",
                table: "doctor_reviews",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_emr_encounters_patient_patient_id",
                table: "emr_encounters",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lab_orders_patient_patient_id",
                table: "lab_orders",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lab_orders_phlebotomist_assigned_phlebotomist_id",
                table: "lab_orders",
                column: "assigned_phlebotomist_id",
                principalTable: "phlebotomists",
                principalColumn: "phlebotomist_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_patient_allergies_patient_patient_id",
                table: "patient_allergies",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "patient_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
