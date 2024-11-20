namespace RaspberryPi.API.DTO;

public class CreateWeekScheduleDto {
	public int Hour { get; set; }
	public int Minute { get; set; }
	public int Second { get; set; }
	public DayOfWeek Day { get; set; }
}
