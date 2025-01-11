using Microsoft.EntityFrameworkCore;
using RaspberryPi.API.Database;
using RaspberryPi.API.Gpio;
using RaspberryPi.API.Options;
using RaspberryPi.API.Repositories;
using RaspberryPi.API.Resolvers;
using RaspberryPi.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen();

builder.Services
	.AddDbContext<DatabaseContext>(options => options.UseSqlite(@"Data Source=Database\Database.db"))
	.AddSingleton<ITestLedService, TestLedService>()
	.AddSingleton<IRaspberryLedService, RaspberryLedService>()
	.AddSingleton<ILedServiceResolver, LedServiceResolver>()
	.AddSingleton<IGpioControllerProvider, GpioControllerProvider>()
	.AddHostedService<ScheduleService>()
	.AddTransient<IWeekScheduleRepository, WeekScheduleDbRepository>();

builder.Services.AddOptions<PinOptions>()
	.Bind(builder.Configuration.GetRequiredSection(nameof(PinOptions)))
	.ValidateOnStart();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

using (IServiceScope scope = app.Services.CreateScope()) {

	var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
	databaseContext.Database.Migrate();
	databaseContext.SaveChanges();
}

app.MapControllers();
app.Run();