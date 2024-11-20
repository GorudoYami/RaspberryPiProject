using RaspberryPi.API.Entities;

namespace RaspberryPi.API.Repositories;

public interface IWeekScheduleRepository {
	Task SaveAsync(WeekSchedule weekSchedule, CancellationToken cancellationToken);
	Task DeleteAsync(int id, CancellationToken cancellationToken);
	Task<IEnumerable<WeekSchedule>> GetAllAsync(CancellationToken cancellationToken);
	Task<WeekSchedule?> GetAsync(int id, CancellationToken cancellationToken);
}
