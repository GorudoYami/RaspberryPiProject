﻿using RaspberryPi.API.Enums;

namespace RaspberryPi.API.DTO;

public class CreateWeekScheduleDto {
	public int? Hour { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Minute { get; set; }
	public int Second { get; set; }
	public DayOfWeek? Day { get; set; }
	public ScheduleAction Action { get; set; }
}
