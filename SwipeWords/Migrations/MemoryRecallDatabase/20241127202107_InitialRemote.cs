using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwipeWords.Migrations.MemoryRecallDatabase
{
    /// <inheritdoc />
    public partial class InitialRemote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeedReadingTexts",
                columns: table => new
                {
                    SpeedReadingTextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedReadingTexts", x => x.SpeedReadingTextId);
                });

            migrationBuilder.CreateTable(
                name: "UserMemoryRecalls",
                columns: table => new
                {
                    MemoryRecallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpeedReadingTextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RemovedWordPositions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMemoryRecalls", x => x.MemoryRecallId);
                    table.ForeignKey(
                        name: "FK_UserMemoryRecalls_SpeedReadingTexts_SpeedReadingTextId",
                        column: x => x.SpeedReadingTextId,
                        principalTable: "SpeedReadingTexts",
                        principalColumn: "SpeedReadingTextId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMemoryRecalls_SpeedReadingTextId",
                table: "UserMemoryRecalls",
                column: "SpeedReadingTextId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMemoryRecalls");

            migrationBuilder.DropTable(
                name: "SpeedReadingTexts");
        }
    }
}
