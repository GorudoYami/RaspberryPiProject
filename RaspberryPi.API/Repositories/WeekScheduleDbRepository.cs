using Microsoft.EntityFrameworkCore;
using RaspberryPi.API.Database;
using RaspberryPi.API.Entities;
using RaspberryPi.API.Exceptions;

namespace RaspberryPi.API.Repositories;

public class WeekScheduleDbRepository(DatabaseContext databaseContext) : IWeekScheduleRepository {
	private readonly DatabaseContext _databaseContext = databaseContext;

	public async Task DeleteAsync(int id, CancellationToken cancellationToken) {
		WeekSchedule weekSchedule = await GetAsync(id, cancellationToken)
			?? throw new NotFoundException();
		_databaseContext.WeekSchedule.Remove(weekSchedule);
		await _databaseContext.SaveChangesAsync(cancellationToken);
	}

	public async Task<IEnumerable<WeekSchedule>> GetAllAsync(CancellationToken cancellationToken) {
		return await _databaseContext.WeekSchedule.ToListAsync(cancellationToken);
	}

	public async Task<WeekSchedule?> GetAsync(int id, CancellationToken cancellationToken) {
		return await _databaseContext.WeekSchedule.FindAsync(id, cancellationToken);
	}

	public async Task SaveAsync(WeekSchedule weekSchedule, CancellationToken cancellationToken) {
		if (weekSchedule.Id == 0) {
			await _databaseContext.WeekSchedule.AddAsync(weekSchedule, cancellationToken);
		}

		await _databaseContext.SaveChangesAsync(cancellationToken);
	}
}
