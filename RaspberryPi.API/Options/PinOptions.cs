using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Options;

public class PinOptions {
	[Required]
	public required int RedPinNumber { get; init; }
	[Required]
	public required int GreenPinNumber { get; init; }
	[Required]
	public required int BluePinNumber { get; init; }
	[Required]
	public int PwmFrequency { get; init; }
}
