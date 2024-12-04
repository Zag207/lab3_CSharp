using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lab3_CSharp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Philosophers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    Die_date = table.Column<DateOnly>(type: "date", nullable: true),
                    IsDie = table.Column<bool>(type: "boolean", nullable: false),
                    CountryLivingId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Philosophers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Philosophers_Countries_CountryLivingId",
                        column: x => x.CountryLivingId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ViewName = table.Column<string>(type: "text", nullable: false),
                    PhilosopherId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Views_Philosophers_PhilosopherId",
                        column: x => x.PhilosopherId,
                        principalTable: "Philosophers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkName = table.Column<string>(type: "text", nullable: false),
                    PhilosopherId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Philosophers_PhilosopherId",
                        column: x => x.PhilosopherId,
                        principalTable: "Philosophers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Philosophers_CountryLivingId",
                table: "Philosophers",
                column: "CountryLivingId");

            migrationBuilder.CreateIndex(
                name: "IX_Views_PhilosopherId",
                table: "Views",
                column: "PhilosopherId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_PhilosopherId",
                table: "Works",
                column: "PhilosopherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Philosophers");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
