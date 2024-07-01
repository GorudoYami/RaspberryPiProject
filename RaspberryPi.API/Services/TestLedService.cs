using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;
using System.Diagnostics;

namespace RaspberryPi.API.Services;

public interface ITestLedService : ILedService {
}

public class TestLedService
	: ITestLedService {
	public Task SetColorAsync(LedColor color) {
		Debug.WriteLine($"[{nameof(TestLedService)}]: SetColor {color}");
		return Task.CompletedTask;
	}

	public Task SetEffectAsync(Effect effect) {
		Debug.WriteLine($"[{nameof(TestLedService)}]: SetEffect {(int)effect} - {effect}");
		return Task.CompletedTask;
	}
}
