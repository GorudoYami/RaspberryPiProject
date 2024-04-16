using GorudoYami.RaspberryPi.Gpio;
using RaspberryPi.API.Models;
using System.Drawing;

namespace RaspberryPi.API.Services;

public interface ILedService {
	void SetSolidColor(LedColor color);
}

public class LedService(IGpioControllerProvider gpioController) : ILedService {
	private readonly IGpioControllerProvider _gpioController = gpioController;
	private Color _currentColor;

	public void SetSolidColor(LedColor color) {

	}
}
