namespace RaspberryPi.API.Entities;

public class WeekSchedule {
	public int Id { get; set; }
	public int Hour { get; set; }
	public int Minute { get; set; }
	public int Second { get; set; }
	public DayOfWeek Day { get; set; }
}
