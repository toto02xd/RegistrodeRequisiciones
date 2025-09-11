using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrodeRequisiciones.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblLoans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantId = table.Column<long>(type: "bigint", nullable: false),
                    ResponsibleId = table.Column<long>(type: "bigint", nullable: false),
                    Article = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedAt = table.Column<DateTime>(type: "datetime2", nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblLoans_TblUsers_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "TblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblLoans_TblUsers_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "TblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblLoans_ApplicantId",
                table: "TblLoans",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_TblLoans_ResponsibleId",
                table: "TblLoans",
                column: "ResponsibleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblLoans");

            migrationBuilder.DropTable(
                name: "TblUsers");
        }
    }
}
