using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPlayer.Database.Repository.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class TagsManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagVideo_Tags_TagsId",
                table: "TagVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_TagVideo_Videos_VideosId",
                table: "TagVideo");

            migrationBuilder.RenameColumn(
                name: "VideosId",
                table: "TagVideo",
                newName: "VideoId");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "TagVideo",
                newName: "TagId");

            migrationBuilder.RenameIndex(
                name: "IX_TagVideo_VideosId",
                table: "TagVideo",
                newName: "IX_TagVideo_VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagVideo_Tags_TagId",
                table: "TagVideo",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagVideo_Videos_VideoId",
                table: "TagVideo",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagVideo_Tags_TagId",
                table: "TagVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_TagVideo_Videos_VideoId",
                table: "TagVideo");

            migrationBuilder.RenameColumn(
                name: "VideoId",
                table: "TagVideo",
                newName: "VideosId");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "TagVideo",
                newName: "TagsId");

            migrationBuilder.RenameIndex(
                name: "IX_TagVideo_VideoId",
                table: "TagVideo",
                newName: "IX_TagVideo_VideosId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagVideo_Tags_TagsId",
                table: "TagVideo",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagVideo_Videos_VideosId",
                table: "TagVideo",
                column: "VideosId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
