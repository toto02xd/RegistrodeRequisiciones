using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrodeRequisiciones.Migrations
{
    /// <inheritdoc />
    public partial class second_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "TblLoans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfEquipment",
                table: "TblLoans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "TblLoans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "model",
                table: "TblLoans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "TblLoans");

            migrationBuilder.DropColumn(
                name: "TypeOfEquipment",
                table: "TblLoans");

            migrationBuilder.DropColumn(
                name: "brand",
                table: "TblLoans");

            migrationBuilder.DropColumn(
                name: "model",
                table: "TblLoans");
        }
    }
}
