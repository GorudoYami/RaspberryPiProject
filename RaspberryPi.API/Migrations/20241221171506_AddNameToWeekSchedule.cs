using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspberryPi.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToWeekSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WeekSchedule",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "WeekSchedule");
        }
    }
}
