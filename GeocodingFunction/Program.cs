using CoreInfrastructure.Services.Geolocation;
using DataAccess.Database;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;
using System;
using DataAccess.Interface;

Log.Logger = new LoggerConfiguration()
		   .MinimumLevel.Debug()
		   .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
		   .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Fatal)

		   .Enrich.FromLogContext()
		   .WriteTo.Console()
		   .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
		   .CreateLogger();

var host = new HostBuilder()
	.ConfigureFunctionsWebApplication()
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();

		services.AddDbContextFactory<DataContext>(options =>
		{
			options.EnableSensitiveDataLogging(false);
			options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:SQLConnectionString"));

		});
		services.AddSingleton(Log.Logger);
		services.AddScoped<IInstitutionRepository, InstitutionRepository>();
		services.AddScoped<IGoogleGeolocationService>(provider => new GoogleGeolocationService(new HttpClient(), Environment.GetEnvironmentVariable("GOOGLE_GEOLOCATION_APIKEY"), "https://maps.googleapis.com/maps/api/geocode/json"));
		services.AddScoped<IKoordinateRepository, KoordinateRepository>();
	})
	.Build();

host.Run();
