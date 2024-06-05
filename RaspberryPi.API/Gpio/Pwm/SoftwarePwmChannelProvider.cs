using System.Device.Gpio;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;

namespace RaspberryPi.API.Gpio.Pwm;

public class SoftwarePwmChannelProvider(GpioController controller, int channel, int frequency, double dutyCyclePercent)
	: IPwmChannelProvider {
	public int Frequency {
		get => _channel.Frequency;
		set => _channel.Frequency = value;
	}
	public double DutyCycle {
		get => _channel.DutyCycle;
		set => _channel.DutyCycle = value;
	}
	private readonly PwmChannel _channel = new SoftwarePwmChannel(channel, frequency, dutyCyclePercent, controller: controller);

	public void Start() {
		_channel.Start();
	}

	public void Stop() {
		_channel.Stop();
	}

	public void Dispose() {
		GC.SuppressFinalize(this);
		_channel.Dispose();
	}
}
