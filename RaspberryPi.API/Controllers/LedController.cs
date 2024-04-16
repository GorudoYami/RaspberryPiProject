using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models;
using RaspberryPi.API.Services;

namespace RaspberryPi.API.Controllers;

[Route("api/leds")]
[ApiController]
public class LedController(ILedService ledService) : ControllerBase {
	private readonly ILedService _ledService = ledService;

	[HttpPost("solid")]
	public ActionResult SetSolidColor(LedColor color) {
		_ledService.SetSolidColor(color);
		return Ok();
	}
}
