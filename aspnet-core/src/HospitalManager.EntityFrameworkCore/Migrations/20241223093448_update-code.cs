using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManager.Migrations
{
    /// <inheritdoc />
    public partial class updatecode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Wards_Code",
                table: "Wards",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_Code",
                table: "Provinces",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Code",
                table: "Patients",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_Code",
                table: "Hospitals",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Code",
                table: "Districts",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wards_Code",
                table: "Wards");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_Code",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Code",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Hospitals_Code",
                table: "Hospitals");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Code",
                table: "Districts");
        }
    }
}
