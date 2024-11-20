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
                    Hour = table.Column<int>(type: "INTEGER", nullable: true),
                    Minute = table.Column<int>(type: "INTEGER", nullable: true),
                    Second = table.Column<int>(type: "INTEGER", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: true),
                    Action = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekSchedule", x => x.Id);
                    table.CheckConstraint("CK_WeekSchedule_Day_Range", "Day BETWEEN 0 AND 6");
                    table.CheckConstraint("CK_WeekSchedule_Hour_Range", "Hour BETWEEN 0 AND 23");
                    table.CheckConstraint("CK_WeekSchedule_Minute_Range", "Minute BETWEEN 0 AND 59");
                    table.CheckConstraint("CK_WeekSchedule_Second_Range", "Second BETWEEN 0 AND 59");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekSchedule");
        }
    }
}
