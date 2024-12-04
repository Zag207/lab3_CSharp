using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab3_CSharp.Migrations
{
    /// <inheritdoc />
    public partial class Addrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Views_Philosophers_PhilosopherId",
                table: "Views");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Philosophers_PhilosopherId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_PhilosopherId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Views_PhilosopherId",
                table: "Views");

            migrationBuilder.DropColumn(
                name: "PhilosopherId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "PhilosopherId",
                table: "Views");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Works",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PhilosopherView",
                columns: table => new
                {
                    PhilosophersId = table.Column<int>(type: "integer", nullable: false),
                    ViewsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhilosopherView", x => new { x.PhilosophersId, x.ViewsId });
                    table.ForeignKey(
                        name: "FK_PhilosopherView_Philosophers_PhilosophersId",
                        column: x => x.PhilosophersId,
                        principalTable: "Philosophers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhilosopherView_Views_ViewsId",
                        column: x => x.ViewsId,
                        principalTable: "Views",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_AuthorId",
                table: "Works",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PhilosopherView_ViewsId",
                table: "PhilosopherView",
                column: "ViewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Philosophers_AuthorId",
                table: "Works",
                column: "AuthorId",
                principalTable: "Philosophers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_Philosophers_AuthorId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "PhilosopherView");

            migrationBuilder.DropIndex(
                name: "IX_Works_AuthorId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Works");

            migrationBuilder.AddColumn<int>(
                name: "PhilosopherId",
                table: "Works",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhilosopherId",
                table: "Views",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Works_PhilosopherId",
                table: "Works",
                column: "PhilosopherId");

            migrationBuilder.CreateIndex(
                name: "IX_Views_PhilosopherId",
                table: "Views",
                column: "PhilosopherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Views_Philosophers_PhilosopherId",
                table: "Views",
                column: "PhilosopherId",
                principalTable: "Philosophers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Philosophers_PhilosopherId",
                table: "Works",
                column: "PhilosopherId",
                principalTable: "Philosophers",
                principalColumn: "Id");
        }
    }
}
