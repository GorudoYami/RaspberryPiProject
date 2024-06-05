using Microsoft.Extensions.Options;
using RaspberryPi.API.Enums;
using RaspberryPi.API.Gpio;
using RaspberryPi.API.Gpio.Pwm;
using RaspberryPi.API.Models;
using RaspberryPi.API.Options;

namespace RaspberryPi.API.Services;

public interface IRaspberryLedService : ILedService {
}

public class RaspberryLedService
	: IRaspberryLedService, IDisposable {
	private readonly IGpioControllerProvider _controller;
	private readonly PinOptions _pinOptions;
	private readonly LedOptions _ledOptions;
	private readonly List<IPwmChannelProvider> _pwmChannels;
	private RainbowStage _rainbowStage;
	private LedColor _currentColor;
	private Effect? _currentEffect;
	private Task _effectTask;
	private CancellationTokenSource? _cts;

	public RaspberryLedService(IOptions<PinOptions> pinOptions, IOptions<LedOptions> ledOptions, IGpioControllerProvider controller) {
		_controller = controller;
		_pinOptions = pinOptions.Value;
		_ledOptions = ledOptions.Value;
		_currentColor = new LedColor() {
			Red = 255,
			Green = 255,
			Blue = 255
		};
		_pwmChannels = [];
		_rainbowStage = RainbowStage.Red;
		_cts = new CancellationTokenSource();
		_effectTask = new Task(ProcessEffect);
		InitializePwmChannels();
	}

	private void InitializePwmChannels() {
		_pwmChannels.Clear();
		_pwmChannels.Add(_controller.GetSoftwarePwmChannel(_pinOptions.RedPinNumber, _pinOptions.PwmFrequency, 0));
		_pwmChannels.Add(_controller.GetSoftwarePwmChannel(_pinOptions.GreenPinNumber, _pinOptions.PwmFrequency, 0));
		_pwmChannels.Add(_controller.GetSoftwarePwmChannel(_pinOptions.BluePinNumber, _pinOptions.PwmFrequency, 0));
	}

	private void ProcessEffect() {
		switch (_currentEffect) {
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
		_pwmChannels[0].DutyCycle = _currentColor.Red / 255.0;
		_pwmChannels[1].DutyCycle = _currentColor.Green / 255.0;
		_pwmChannels[2].DutyCycle = _currentColor.Blue / 255.0;
	}

	private void UpdatePulse() {
		CancellationToken token = _cts!.Token;
		while (token.IsCancellationRequested == false) {
			_pwmChannels[0].DutyCycle = _currentColor.Red / 255.0;
			_pwmChannels[1].DutyCycle = _currentColor.Green / 255.0;
			_pwmChannels[2].DutyCycle = _currentColor.Blue / 255.0;
			Thread.Sleep(500);
			_pwmChannels[0].DutyCycle = 0;
			_pwmChannels[1].DutyCycle = 0;
			_pwmChannels[2].DutyCycle = 0;
			Thread.Sleep(500);
		}
	}

	private void UpdateFade() {
		CancellationToken token = _cts!.Token;
		while (token.IsCancellationRequested == false) {
			for (int i = 0; i <= 100; i++) {
				_pwmChannels[0].DutyCycle = _currentColor.Red / 255.0 * (i / 100.0);
				_pwmChannels[1].DutyCycle = _currentColor.Green / 255.0 * (i / 100.0);
				_pwmChannels[2].DutyCycle = _currentColor.Blue / 255.0 * (i / 100.0);
				Thread.Sleep(5);
			}
			for (int i = 100; i >= 0; i--) {
				_pwmChannels[0].DutyCycle = _currentColor.Red / 255.0 * (i / 100.0);
				_pwmChannels[1].DutyCycle = _currentColor.Green / 255.0 * (i / 100.0);
				_pwmChannels[2].DutyCycle = _currentColor.Blue / 255.0 * (i / 100.0);
				Thread.Sleep(5);
			}
		}
	}

	private void UpdateRainbow() {
		CancellationToken token = _cts!.Token;
		while (token.IsCancellationRequested == false) {

		}
	}

	public void SetColor(LedColor color) {
		_currentColor = color;
	}

	public async Task SetEffect(Effect effect) {
		if (_effectTask != null) {
			_cts!.Cancel();
			await _effectTask;
			_cts.Dispose();
			_cts = null;
		}

		_currentEffect = effect;
		if (_currentEffect != Effect.Solid) {
			_effectTask = Task.Run(ProcessEffect);
		}
		else {
			UpdateSolidColor();
		}
	}

	public void Dispose() {
		GC.SuppressFinalize(this);
		if (_effectTask != null) {
			_cts!.Cancel();
			_effectTask.GetAwaiter().GetResult();
			_cts.Dispose();
		}
	}
}
