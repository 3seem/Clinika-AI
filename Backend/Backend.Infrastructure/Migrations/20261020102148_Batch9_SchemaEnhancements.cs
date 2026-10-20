using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Batch9_SchemaEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "drug_id",
                table: "prescription_items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "custom_drug_name",
                table: "prescription_items",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "prescription_item_id",
                table: "pharmacy_order_items",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "published_at",
                table: "lab_orders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "report_pdf_url",
                table: "lab_orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "follow_up_date",
                table: "emr_encounters",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "clinic_address",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "clinic_city",
                table: "doctors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "clinic_lat",
                table: "doctors",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "clinic_lng",
                table: "doctors",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "clinic_name",
                table: "doctors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "doctors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "profile_picture_url",
                table: "doctors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "doctors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Specialist");

            migrationBuilder.AddColumn<string>(
                name: "consultation_type",
                table: "doctor_schedules",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "both");

            migrationBuilder.AddColumn<string>(
                name: "reason_for_visit",
                table: "appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telehealth_room_id",
                table: "appointments",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_DocSchedule_Type",
                table: "doctor_schedules",
                sql: "[consultation_type] IN ('clinic', 'telehealth', 'both')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DocSchedule_Type",
                table: "doctor_schedules");

            migrationBuilder.DropColumn(
                name: "custom_drug_name",
                table: "prescription_items");

            migrationBuilder.DropColumn(
                name: "published_at",
                table: "lab_orders");

            migrationBuilder.DropColumn(
                name: "report_pdf_url",
                table: "lab_orders");

            migrationBuilder.DropColumn(
                name: "follow_up_date",
                table: "emr_encounters");

            migrationBuilder.DropColumn(
                name: "clinic_address",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "clinic_city",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "clinic_lat",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "clinic_lng",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "clinic_name",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "full_name",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "profile_picture_url",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "title",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "consultation_type",
                table: "doctor_schedules");

            migrationBuilder.DropColumn(
                name: "reason_for_visit",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "telehealth_room_id",
                table: "appointments");

            migrationBuilder.AlterColumn<int>(
                name: "drug_id",
                table: "prescription_items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "prescription_item_id",
                table: "pharmacy_order_items",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
