using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPlayer.Database.Repository.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class TagsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Videos_VideoId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_VideoId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "TagVideo",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false),
                    VideosId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagVideo", x => new { x.TagsId, x.VideosId });
                    table.ForeignKey(
                        name: "FK_TagVideo_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagVideo_Videos_VideosId",
                        column: x => x.VideosId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagVideo_VideosId",
                table: "TagVideo",
                column: "VideosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagVideo");

            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "Tags",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_VideoId",
                table: "Tags",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Videos_VideoId",
                table: "Tags",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
