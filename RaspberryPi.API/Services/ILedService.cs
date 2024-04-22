using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;

namespace RaspberryPi.API.Services;

public interface ILedService {
	void SetColor(LedColor color);
	Task SetEffect(Effect effect);
}
