using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Batch2_MasterData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "drugs_master",
                columns: table => new
                {
                    drug_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    generic_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    form = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    standard_strength = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    atc_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drugs_master", x => x.drug_id);
                });

            migrationBuilder.CreateTable(
                name: "lab_tests_master",
                columns: table => new
                {
                    test_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loinc_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    test_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    standard_unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    reference_range_male = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    reference_range_female = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lab_tests_master", x => x.test_id);
                });

            migrationBuilder.CreateTable(
                name: "medical_specialties",
                columns: table => new
                {
                    specialty_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_medical_specialties", x => x.specialty_id);
                });

            migrationBuilder.CreateTable(
                name: "drug_interactions",
                columns: table => new
                {
                    interaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    drug_a_id = table.Column<int>(type: "int", nullable: false),
                    drug_b_id = table.Column<int>(type: "int", nullable: false),
                    severity_level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drug_interactions", x => x.interaction_id);
                    table.CheckConstraint("CK_drug_interactions_severity", "[severity_level] IN ('minor','moderate','severe','contraindicated')");
                    table.CheckConstraint("CK_DrugInteraction_Self", "[drug_a_id] <> [drug_b_id]");
                    table.ForeignKey(
                        name: "fk_drug_interactions_drugs_master_drug_a_id",
                        column: x => x.drug_a_id,
                        principalTable: "drugs_master",
                        principalColumn: "drug_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_drug_interactions_drugs_master_drug_b_id",
                        column: x => x.drug_b_id,
                        principalTable: "drugs_master",
                        principalColumn: "drug_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_drug_interactions_drug_a_id",
                table: "drug_interactions",
                column: "drug_a_id");

            migrationBuilder.CreateIndex(
                name: "ix_drug_interactions_drug_b_id",
                table: "drug_interactions",
                column: "drug_b_id");

            migrationBuilder.CreateIndex(
                name: "ix_medical_specialties_name",
                table: "medical_specialties",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drug_interactions");

            migrationBuilder.DropTable(
                name: "lab_tests_master");

            migrationBuilder.DropTable(
                name: "medical_specialties");

            migrationBuilder.DropTable(
                name: "drugs_master");
        }
    }
}
