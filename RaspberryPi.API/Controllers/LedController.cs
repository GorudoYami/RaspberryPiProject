using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;
using RaspberryPi.API.Resolvers;
using RaspberryPi.API.Services;

namespace RaspberryPi.API.Controllers;

[Route("api/leds")]
[ApiController]
public class LedController(ILedServiceResolver ledServiceResolver) : ControllerBase {
	private readonly ILedService _ledService = ledServiceResolver.Resolve();

	[HttpPost("effect")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> SetEffectAsync([FromBody] int effect) {
		await _ledService.SetEffectAsync((Effect)effect);
		return Ok();
	}

	[HttpPost("color")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> SetColorAsync(LedColor color) {
		await _ledService.SetColorAsync(color);
		return Ok();
	}
}
