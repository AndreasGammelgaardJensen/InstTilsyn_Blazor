using CoreInfrastructure.Services.Geolocation;
using DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using TilsynsRapportApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configbuilder = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddEnvironmentVariables();


IConfigurationRoot configuration = Configbuilder.Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<DataContext>(options =>
{
    options.EnableSensitiveDataLogging(false);
    options.UseSqlServer(configuration["ConnectionStrings:SQLServerConnection"], options => options.EnableRetryOnFailure().CommandTimeout(60));

});

builder.Services.AddTransient<DataAccess.Interface.IInstitutionTableRepository, InstitutionTabelReporisory>();
builder.Services.AddScoped<IGoogleGeolocationService>(provider => new GoogleGeolocationService(new HttpClient(), configuration["GOOGLE_GEOLOCATION_APIKEY"], "https://maps.googleapis.com/maps/api/geocode/json"));

builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
