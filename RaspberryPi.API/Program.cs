using RaspberryPi.API.Gpio;
using RaspberryPi.API.Options;
using RaspberryPi.API.Resolvers;
using RaspberryPi.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen();

builder.Services
	.AddSingleton<ITestLedService, TestLedService>()
	.AddSingleton<IRaspberryLedService, RaspberryLedService>()
	.AddSingleton<ILedServiceResolver, LedServiceResolver>()
	.AddSingleton<IGpioControllerProvider, GpioControllerProvider>();

builder.Services.AddOptions<PinOptions>()
	.Bind(builder.Configuration.GetRequiredSection(nameof(PinOptions)))
	.ValidateOnStart();
builder.Services.AddOptions<LedOptions>()
	.Bind(builder.Configuration.GetRequiredSection(nameof(LedOptions)))
	.ValidateOnStart();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();
app.Run();