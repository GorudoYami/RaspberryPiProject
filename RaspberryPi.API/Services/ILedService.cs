using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;

namespace RaspberryPi.API.Services;

public interface ILedService {
	Task SetColorAsync(LedColor color);
	Task SetEffectAsync(Effect effect);
}
