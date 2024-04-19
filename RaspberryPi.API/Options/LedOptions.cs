using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Options;

public class LedOptions {
	[Required]
	public int RefreshPeriod { get; init; }
}
