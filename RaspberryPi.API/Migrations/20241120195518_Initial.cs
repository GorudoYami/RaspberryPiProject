using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspberryPi.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeekSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hour = table.Column<int>(type: "INTEGER", nullable: false),
                    Minute = table.Column<int>(type: "INTEGER", nullable: false),
                    Second = table.Column<int>(type: "INTEGER", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekSchedule", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeekSchedule_Day_Hour_Minute_Second",
                table: "WeekSchedule",
                columns: new[] { "Day", "Hour", "Minute", "Second" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekSchedule");
        }
    }
}
