
using RaspberryPi.API.Entities;
using RaspberryPi.API.Enums;
using RaspberryPi.API.Repositories;
using RaspberryPi.API.Resolvers;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace RaspberryPi.API.Services;

public class ScheduleService(IServiceProvider serviceProvider, ILedServiceResolver ledServiceResolver)
	: IHostedService {
	private readonly IServiceProvider _serviceProvider = serviceProvider;
	private readonly ILedService _ledService = ledServiceResolver.Resolve();
	private readonly List<Timer> _scheduledTimers = [];
	private readonly Timer _updateTimer = new();

	public Task StartAsync(CancellationToken cancellationToken) {
		_updateTimer.Interval = 5000;
		_updateTimer.AutoReset = true;
		_updateTimer.Elapsed += async (s, e) => await UpdateAsync(cancellationToken);
		_updateTimer.Start();

		return Task.CompletedTask;
	}

	private async Task UpdateAsync(CancellationToken cancellationToken) {
		_scheduledTimers.ForEach(timer => timer.Stop());
		_scheduledTimers.ForEach(timer => timer.Dispose());
		_scheduledTimers.Clear();

		using var scope = _serviceProvider.CreateScope();
		IEnumerable<WeekSchedule> schedules = await scope.ServiceProvider
			.GetRequiredService<IWeekScheduleRepository>()
			.GetAllAsync(cancellationToken);
		ScheduleTimers(schedules);
	}

	public void ScheduleTimers(IEnumerable<WeekSchedule> schedules) {
		var now = DateTime.Now;

		foreach (var schedule in schedules) {
			if (schedule.Day.HasValue && schedule.Hour.HasValue && schedule.Minute.HasValue) {
				ScheduleSpecificTime(schedule, now);
			}
			else if (!schedule.Day.HasValue && schedule.Hour.HasValue && schedule.Minute.HasValue) {
				ScheduleDaily(schedule, now);
			}
			else if (!schedule.Day.HasValue && !schedule.Hour.HasValue && schedule.Minute.HasValue) {
				ScheduleHourly(schedule, now);
			}
			else if (!schedule.Day.HasValue && !schedule.Hour.HasValue && !schedule.Minute.HasValue) {
				ScheduleMinutely(schedule, now);
			}
		}
	}

	private void ScheduleSpecificTime(WeekSchedule schedule, DateTime now) {
		DateTime nextOccurrence = GetNextOccurrence(
			now,
			schedule.Day!.Value,
			schedule.Hour!.Value,
			schedule.Minute!.Value,
			schedule.Second
		);

		if (nextOccurrence <= now) return;

		double delayMilliseconds = (nextOccurrence - now).TotalMilliseconds;
		CreateTimer(schedule, delayMilliseconds, Timeout.Infinite);
		Debug.WriteLine($"Scheduled '{schedule.Name}' for {nextOccurrence}");
	}

	private void ScheduleDaily(WeekSchedule schedule, DateTime now) {
		DateTime nextOccurrence = GetNextOccurrence(
			now,
			now.DayOfWeek,
			schedule.Hour!.Value,
			schedule.Minute!.Value,
			schedule.Second
		);

		if (nextOccurrence <= now)
			nextOccurrence = nextOccurrence.AddDays(1);

		double delayMilliseconds = (nextOccurrence - now).TotalMilliseconds;
		CreateTimer(schedule, delayMilliseconds, 24 * 60 * 60 * 1000);
		Debug.WriteLine($"Scheduled '{schedule.Name}' daily at {nextOccurrence.TimeOfDay}");
	}

	private void ScheduleHourly(WeekSchedule schedule, DateTime now) {
		var nextOccurrence = new DateTime(
			now.Year,
			now.Month,
			now.Day,
			now.Hour,
			schedule.Minute!.Value,
			schedule.Second
		);

		if (nextOccurrence <= now)
			nextOccurrence = nextOccurrence.AddHours(1);

		double delayMilliseconds = (nextOccurrence - now).TotalMilliseconds;
		CreateTimer(schedule, delayMilliseconds, 60 * 60 * 1000);
		Debug.WriteLine($"Scheduled '{schedule.Name}' hourly at minute {schedule.Minute.Value}");
	}

	private void ScheduleMinutely(WeekSchedule schedule, DateTime now) {
		var nextOccurrence = new DateTime(
			now.Year,
			now.Month,
			now.Day,
			now.Hour,
			now.Minute,
			schedule.Second
		);

		if (nextOccurrence <= now) {
			nextOccurrence = nextOccurrence.AddMinutes(1);
		}

		double delayMilliseconds = (nextOccurrence - now).TotalMilliseconds;
		CreateTimer(schedule, delayMilliseconds, 60 * 1000);
		Debug.WriteLine($"Scheduled '{schedule.Name}' every minute at second {schedule.Second}");
	}

	private static DateTime GetNextOccurrence(DateTime now, DayOfWeek targetDay, int hour, int minute, int second) {
		int daysUntilTarget = ((int)targetDay - (int)now.DayOfWeek + 7) % 7;
		DateTime nextOccurrence = now.Date.AddDays(daysUntilTarget).AddHours(hour).AddMinutes(minute).AddSeconds(second);
		return nextOccurrence;
	}

	private void CreateTimer(WeekSchedule schedule, double dueTime, double interval) {
		var timer = new Timer {
			Interval = dueTime,
			AutoReset = false
		};

		timer.Elapsed += (sender, args) => {
			ExecuteAction(schedule.Action);

			if (interval > 0) {
				timer.Interval = interval;
				timer.AutoReset = true;
				timer.Start();
			}
		};

		timer.Start();
		_scheduledTimers.Add(timer);
	}

	private void ExecuteAction(ScheduleAction action) {
		switch (action) {
			case ScheduleAction.TurnOn:
				_ledService.Enabled = true;
				break;
			case ScheduleAction.TurnOff:
				_ledService.Enabled = false;
				break;
		}
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_updateTimer.Stop();

		_scheduledTimers.ForEach(timer => timer.Stop());
		_scheduledTimers.ForEach(timer => timer.Dispose());
		_scheduledTimers.Clear();

		return Task.CompletedTask;
	}
}
