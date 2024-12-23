using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManager.Migrations
{
    /// <inheritdoc />
    public partial class updatedelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Wards",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Wards",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "UserHospitals",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "UserHospitals",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserHospitals",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Provinces",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Provinces",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Patients",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Patients",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Hospitals",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Hospitals",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Districts",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Districts",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "UserHospitals");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "UserHospitals");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserHospitals");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Districts");
        }
    }
}
