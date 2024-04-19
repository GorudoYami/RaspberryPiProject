using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;
using RaspberryPi.API.Services;
using System.Net;

namespace RaspberryPi.API.Controllers;

[Route("api/leds")]
[ApiController]
public class LedController(ILedService ledService) : ControllerBase {
	private readonly ILedService _ledService = ledService;

	[HttpPost("effect")]
	[ProducesResponseType(200)]
	public ActionResult SetEffect(Effect effect) {
		_ledService.SetEffect(effect);
		return Ok();
	}

	[HttpPost("color")]
	[ProducesResponseType(200)]
	public ActionResult SetColor(LedColor color) {
		_ledService.SetColor(color);
		return Ok();
	}
}
