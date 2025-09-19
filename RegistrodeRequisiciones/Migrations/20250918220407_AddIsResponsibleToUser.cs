using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrodeRequisiciones.Migrations
{
    /// <inheritdoc />
    public partial class AddIsResponsibleToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsResponsible",
                table: "TblUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResponsible",
                table: "TblUsers");
        }
    }
}
