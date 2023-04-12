using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPlayer.Database.Repository.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class ContactSheetColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactSheet",
                table: "Videos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactSheet",
                table: "Videos");
        }
    }
}
