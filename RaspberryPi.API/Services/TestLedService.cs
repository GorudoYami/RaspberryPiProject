using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;
using System.Diagnostics;

namespace RaspberryPi.API.Services;

public interface ITestLedService : ILedService {
}

public class TestLedService
	: ITestLedService {
	public void SetColor(LedColor color) {
		Debug.WriteLine($"[{nameof(TestLedService)}]: SetColor {color}");
	}

	public Task SetEffect(Effect effect) {
		Debug.WriteLine($"[{nameof(TestLedService)}]: SetEffect {(int)effect} - {effect}");
		return Task.CompletedTask;
	}
}
