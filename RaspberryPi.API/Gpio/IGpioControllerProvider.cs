using RaspberryPi.API.Gpio.Pwm;
using System.Device;
using System.Device.Gpio;

namespace RaspberryPi.API.Gpio;

public interface IGpioControllerProvider {
	void ClosePin(int pinNumber);
	PinMode GetPinMode(int pinNumber);
	IPwmChannelProvider GetPwmChannel(int chip, int channel, int frequency, double dutyCyclePercentage);
	IPwmChannelProvider GetSoftwarePwmChannel(int channel, int frequency, double dutyCyclePercent);
	bool IsPinModeSupported(int pinNumber, PinMode mode);
	GpioPin OpenPin(int pinNumber);
	GpioPin OpenPin(int pinNumber, PinMode pinMode);
	GpioPin OpenPin(int pinNumber, PinMode pinMode, PinValue initialValue);
	ComponentInformation QueryComponentInformation();
	PinValue Read(int pinNumber);
	void Read(Span<PinValuePair> pinValuePairs);
	void SetPinMode(int pinNumber, PinMode mode);
	void Subscribe(int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback);
	void Toggle(int pinNumber);
	void Unsubscribe(int pinNumber, PinChangeEventHandler callback);
	WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken = default);
	ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken = default);
	void Write(ReadOnlySpan<PinValuePair> pinValuePairs);
	void Write(int pinNumber, PinValue value);
}
