using Microsoft.Extensions.Options;
using RaspberryPi.API.Enums;
using RaspberryPi.API.Gpio;
using RaspberryPi.API.Gpio.Pwm;
using RaspberryPi.API.Models;
using RaspberryPi.API.Options;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RaspberryPi.API.Services;

public interface ILedService {
	void SetColor(LedColor color);
	void SetEffect(Effect effect);
}

public class LedService
	: ILedService, IDisposable {
	private readonly IGpioControllerProvider _controller;
	private readonly Timer _updateTimer;
	private readonly PinOptions _pinOptions;
	private readonly LedOptions _ledOptions;
	private readonly List<IPwmChannelProvider> _pwmChannels;
	private RainbowStage _rainbowStage;
	private LedColor _currentColor;
	private Effect? _currentEffect;

	public LedService(IOptions<PinOptions> pinOptions, IOptions<LedOptions> ledOptions, IGpioControllerProvider controller) {
		_controller = controller;
		_pinOptions = pinOptions.Value;
		_ledOptions = ledOptions.Value;
		_currentColor = new LedColor(255, 255, 255);
		_pwmChannels = [];
		_rainbowStage = RainbowStage.Red;
		InitializePwmChannels();

		_updateTimer = new Timer(_ledOptions.RefreshPeriod) {
			AutoReset = true,
			Enabled = true
		};
		_updateTimer.Elapsed += Update;
	}

	private void InitializePwmChannels() {
		_pwmChannels.Clear();
		_pwmChannels.Add(_controller.GetPwmChannel(1, _pinOptions.RedPinNumber, _pinOptions.PwmFrequency, 0));
		_pwmChannels.Add(_controller.GetPwmChannel(1, _pinOptions.GreenPinNumber, _pinOptions.PwmFrequency, 0));
		_pwmChannels.Add(_controller.GetPwmChannel(1, _pinOptions.BluePinNumber, _pinOptions.PwmFrequency, 0));
	}

	private void Update(object? sender, ElapsedEventArgs e) {
		switch (_currentEffect) {
			case Effect.Solid:
				UpdateSolidColor();
				break;
			case Effect.Pulse:
				UpdatePulse();
				break;
			case Effect.Fade:
				UpdateFade();
				break;
			case Effect.Rainbow:
				UpdateRainbow();
				break;
			default:
				throw new NotImplementedException("Effect not implemented");
		}
	}

	private void UpdateSolidColor() {
		_pwmChannels[0].DutyCycle = _currentColor.Red / 255.0 * 100.0;
		_pwmChannels[1].DutyCycle = _currentColor.Green / 255.0 * 100.0;
		_pwmChannels[2].DutyCycle = _currentColor.Blue / 255.0 * 100.0;
	}

	private void UpdatePulse() {
		throw new NotImplementedException();
	}

	private void UpdateFade() {
		throw new NotImplementedException();
	}

	private void UpdateRainbow() {
		throw new NotImplementedException();
	}

	public void SetColor(LedColor color) {
		_currentColor = color;
	}

	public void SetEffect(Effect effect) {
		_currentEffect = effect;
	}

	public void Dispose() {
		GC.SuppressFinalize(this);
		_updateTimer.Stop();
		_updateTimer.Dispose();
	}
}
