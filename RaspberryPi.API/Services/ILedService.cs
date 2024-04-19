using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;

namespace RaspberryPi.API.Services;

public interface ILedService {
	void SetColor(LedColor color);
	void SetEffect(Effect effect);
}
