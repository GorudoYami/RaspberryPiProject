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
	private readonly List<IPwmChannelProvider> _pwmChannels;
	private LedColor _currentColor;
	private Effect? _currentEffect;
	private Task _effectTask;
	private CancellationTokenSource? _cts;

	public RaspberryLedService(IOptions<PinOptions> pinOptions, IGpioControllerProvider controller) {
		_controller = controller;
		_pinOptions = pinOptions.Value;
		_currentColor = new LedColor() {
			Red = 255,
			Green = 255,
			Blue = 255
		};
		_pwmChannels = [];
		_cts = new CancellationTokenSource();
		_effectTask = new Task(ProcessEffect);
		InitializePwmChannels();
	}

	private void InitializePwmChannels() {
		_pwmChannels.Clear();
		_pwmChannels.Add(_controller.GetSoftwarePwmChannel(_pinOptions.RedPinNumber, _pinOptions.PwmFrequency, 0));
		_pwmChannels.Add(_controller.GetSoftwarePwmChannel(_pinOptions.GreenPinNumber, _pinOptions.PwmFrequency, 0));
		_pwmChannels.Add(_controller.GetSoftwarePwmChannel(_pinOptions.BluePinNumber, _pinOptions.PwmFrequency, 0));

		_pwmChannels.ForEach(pwmChannel => pwmChannel.Start());
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

	private void UpdateColor() {
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
			if (_currentColor.Red == 255 && _currentColor.Green < 255 && _currentColor.Blue == 0) {
				_currentColor.Green += 1;
			}
			else if (_currentColor.Green == 255 && _currentColor.Red > 0 && _currentColor.Blue == 0) {
				_currentColor.Red -= 1;
			}
			else if (_currentColor.Green == 255 && _currentColor.Blue < 255 && _currentColor.Red == 0) {
				_currentColor.Blue += 1;
			}
			else if (_currentColor.Blue == 255 && _currentColor.Green > 0 && _currentColor.Red == 0) {
				_currentColor.Green -= 1;
			}
			else if (_currentColor.Blue == 255 && _currentColor.Red < 255 && _currentColor.Green == 0) {
				_currentColor.Red += 1;
			}
			else if (_currentColor.Red == 255 && _currentColor.Blue > 0 && _currentColor.Green == 0) {
				_currentColor.Blue -= 1;
			}

			UpdateColor();
			Thread.Sleep(5);
		}
	}

	public async Task SetColorAsync(LedColor color) {
		_currentColor = color;
		if (_currentEffect != Effect.Solid) {
			await SetEffectAsync(Effect.Solid);
		}
		else {
			UpdateColor();
		}
	}

	public async Task SetEffectAsync(Effect effect) {
		if (_effectTask?.Status == TaskStatus.Running) {
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
			UpdateColor();
		}
	}

	public void Dispose() {
		GC.SuppressFinalize(this);
		if (_effectTask?.Status == TaskStatus.Running) {
			_cts!.Cancel();
			_effectTask.GetAwaiter().GetResult();
			_cts.Dispose();
		}
		_pwmChannels.ForEach(x => x.Stop());
		_pwmChannels.ForEach(x => x.Dispose());
	}
}
