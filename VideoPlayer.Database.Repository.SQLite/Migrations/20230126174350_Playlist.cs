using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPlayer.Database.Repository.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class Playlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thumbnails_Videos_VideoId",
                table: "Thumbnails");

            migrationBuilder.DropColumn(
                name: "DateAddedString",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Directory",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LengthString",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Preview",
                table: "Videos");

            migrationBuilder.AddColumn<int>(
                name: "DirectoryId",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Length",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "Thumbnails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Thumbnails",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "Directories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DirectoryPath = table.Column<string>(type: "TEXT", nullable: true),
                    DirectoryName = table.Column<string>(type: "TEXT", nullable: true),
                    IsIncludeSubdirectories = table.Column<bool>(type: "INTEGER", nullable: false),
                    MediaType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayListItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VideoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    PlaylistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayListItem_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayListItem_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_DirectoryId",
                table: "Videos",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListItem_PlaylistId",
                table: "PlayListItem",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListItem_VideoId",
                table: "PlayListItem",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thumbnails_Videos_VideoId",
                table: "Thumbnails",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Directories_DirectoryId",
                table: "Videos",
                column: "DirectoryId",
                principalTable: "Directories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thumbnails_Videos_VideoId",
                table: "Thumbnails");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Directories_DirectoryId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Directories");

            migrationBuilder.DropTable(
                name: "PlayListItem");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Videos_DirectoryId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "DirectoryId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Videos");

            migrationBuilder.AddColumn<string>(
                name: "DateAddedString",
                table: "Videos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Directory",
                table: "Videos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LengthString",
                table: "Videos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Preview",
                table: "Videos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "Thumbnails",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Thumbnails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Thumbnails_Videos_VideoId",
                table: "Thumbnails",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
