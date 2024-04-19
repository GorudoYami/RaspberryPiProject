﻿namespace RaspberryPi.API.Gpio.Pwm;
public interface IPwmChannelProvider : IDisposable {
	int Frequency { get; set; }
	double DutyCycle { get; set; }

	void Start();
	void Stop();
}
