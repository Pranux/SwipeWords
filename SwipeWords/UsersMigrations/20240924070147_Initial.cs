#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SwipeWords.UsersMigrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Users",
            table => new
            {
                UserId = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>("nvarchar(max)", nullable: false),
                Email = table.Column<string>("nvarchar(max)", nullable: false),
                CreationDate = table.Column<DateTime>("datetime2", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Users", x => x.UserId); });

        migrationBuilder.CreateTable(
            "Leaderboards",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                MaxScore = table.Column<int>("int", nullable: false),
                RankPosition = table.Column<int>("int", nullable: false),
                UserId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Leaderboards", x => x.Id);
                table.ForeignKey(
                    "FK_Leaderboards_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "UserId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Leaderboards_UserId",
            "Leaderboards",
            "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Leaderboards");

        migrationBuilder.DropTable(
            "Users");
    }
}