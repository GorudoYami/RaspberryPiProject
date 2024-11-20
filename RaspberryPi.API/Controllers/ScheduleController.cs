using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.DTO;
using RaspberryPi.API.Entities;
using RaspberryPi.API.Repositories;
using System.Net;

namespace RaspberryPi.API.Controllers;

[Route("api/schedule")]
[ApiController]
public class ScheduleController(IWeekScheduleRepository weekScheduleRepository)
	: ControllerBase {
	private readonly IWeekScheduleRepository _weekScheduleRepository = weekScheduleRepository;

	[HttpGet]
	public async Task<ActionResult<IEnumerable<WeekSchedule>>> GetAllAsync() {
		return Ok(await _weekScheduleRepository.GetAllAsync(HttpContext.RequestAborted));
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<WeekSchedule>> GetAsync(int id) {
		WeekSchedule? weekSchedule = await _weekScheduleRepository.GetAsync(id, HttpContext.RequestAborted);
		if (weekSchedule == null) {
			return NotFound();
		}

		return Ok(weekSchedule);
	}

	[HttpPost]
	public async Task<ActionResult<WeekSchedule>> CreateAsync(CreateWeekScheduleDto weekScheduleDto) {
		var weekSchedule = new WeekSchedule() {
			Hour = weekScheduleDto.Hour,
			Minute = weekScheduleDto.Minute,
			Second = weekScheduleDto.Second,
			Day = weekScheduleDto.Day,
		};

		try {
			await _weekScheduleRepository.SaveAsync(weekSchedule, HttpContext.RequestAborted);
		}
		catch (Exception ex) {
			return Problem(ex.InnerException?.Message ?? ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
		}

		return Ok(weekSchedule);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAsync(int id) {
		try {
			await _weekScheduleRepository.DeleteAsync(id, HttpContext.RequestAborted);
		}
		catch (Exception) {
			return NotFound();
		}

		return Ok();
	}
}
