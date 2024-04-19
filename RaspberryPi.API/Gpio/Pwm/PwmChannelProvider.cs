﻿using System.Device.Pwm;

namespace RaspberryPi.API.Gpio.Pwm;
public class PwmChannelProvider(int chip, int channel, int frequency, double dutyCyclePercentage)
	: IPwmChannelProvider {
	public int Frequency {
		get => _channel.Frequency;
		set => _channel.Frequency = value;
	}
	public double DutyCycle {
		get => _channel.DutyCycle;
		set => _channel.DutyCycle = value;
	}

	private readonly PwmChannel _channel = PwmChannel.Create(chip, channel, frequency, dutyCyclePercentage);

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
