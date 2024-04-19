namespace RaspberryPi.API.Models;

public class LedColor {
	public byte Red { get; set; }
	public byte Green { get; set; }
	public byte Blue { get; set; }

	public override string ToString() {
		return $"R = {Red}, G = {Green}, B = {Blue}";
	}
}
