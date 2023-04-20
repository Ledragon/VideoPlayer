using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPlayer.Database.Repository.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class ContactSheetSplit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactSheet",
                table: "Videos");

            migrationBuilder.CreateTable(
                name: "ContactSheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VideoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactSheets_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactSheets_VideoId",
                table: "ContactSheets",
                column: "VideoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactSheets");

            migrationBuilder.AddColumn<string>(
                name: "ContactSheet",
                table: "Videos",
                type: "TEXT",
                nullable: true);
        }
    }
}
