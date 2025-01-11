using RaspberryPi.API.Enums;
using RaspberryPi.API.Models;
using System.Diagnostics;

namespace RaspberryPi.API.Services;

public interface ITestLedService : ILedService {
}

public class TestLedService
	: ITestLedService {
	public bool Enabled {
		get {
			Debug.WriteLine($"[{nameof(TestLedService)}]: Enabled {_enabled}");
			return _enabled;
		}
		set {
			Debug.WriteLine($"[{nameof(TestLedService)}]: Set Enabled {value}");
			_enabled = value;
		}
	}
	private bool _enabled;

	public Task SetColorAsync(LedColor color) {
		Debug.WriteLine($"[{nameof(TestLedService)}]: SetColor {color}");
		return Task.CompletedTask;
	}

	public Task SetEffectAsync(Effect effect) {
		Debug.WriteLine($"[{nameof(TestLedService)}]: SetEffect {(int)effect} - {effect}");
		return Task.CompletedTask;
	}
}
