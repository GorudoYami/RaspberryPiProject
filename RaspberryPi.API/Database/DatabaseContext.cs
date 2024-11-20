using Microsoft.EntityFrameworkCore;
using RaspberryPi.API.Entities;

namespace RaspberryPi.API.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options) {
	public DbSet<WeekSchedule> WeekSchedule { get; init; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<WeekSchedule>(entity => {
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => new { e.Day, e.Hour, e.Minute, e.Second }).IsUnique();
		});
	}
}
