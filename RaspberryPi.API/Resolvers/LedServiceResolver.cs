using RaspberryPi.API.Services;

namespace RaspberryPi.API.Resolvers;

public interface ILedServiceResolver {
	ILedService Resolve();
}

public class LedServiceResolver(IWebHostEnvironment environment, IServiceProvider serviceProvider) : ILedServiceResolver {
	private readonly IWebHostEnvironment _environment = environment;
	private readonly IServiceProvider _serviceProvider = serviceProvider;

	public ILedService Resolve() {
		if (_environment.IsDevelopment()) {
			return _serviceProvider.GetRequiredService<ITestLedService>();
		}
		else {
			return _serviceProvider.GetRequiredService<IRaspberryLedService>();
		}
	}
}
