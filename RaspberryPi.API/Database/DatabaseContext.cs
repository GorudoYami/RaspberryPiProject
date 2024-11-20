using Microsoft.EntityFrameworkCore;
using RaspberryPi.API.Entities;

namespace RaspberryPi.API.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options) {
	public DbSet<WeekSchedule> WeekSchedule { get; init; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<WeekSchedule>(entity => {
			entity.HasKey(e => e.Id);
			entity.ToTable(table => {
				table.HasCheckConstraint("CK_WeekSchedule_Hour_Range", "Hour BETWEEN 0 AND 23");
				table.HasCheckConstraint("CK_WeekSchedule_Minute_Range", "Minute BETWEEN 0 AND 59");
				table.HasCheckConstraint("CK_WeekSchedule_Second_Range", "Second BETWEEN 0 AND 59");
				table.HasCheckConstraint("CK_WeekSchedule_Day_Range", "Day BETWEEN 0 AND 6");
			});
		});
	}
}