using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;

namespace RaspberryPi.API.Services;

public interface ILedService {
	bool Enabled { get; set; }

	Task SetColorAsync(LedColor color);
	Task SetEffectAsync(Effect effect);
}
