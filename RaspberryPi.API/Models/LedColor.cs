namespace RaspberryPi.API.Models;

public class LedColor(byte r, byte g, byte b) {
	public byte Red { get; set; } = r;
	public byte Green { get; set; } = g;
	public byte Blue { get; set; } = b;
}
