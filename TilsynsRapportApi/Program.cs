using DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using TilsynsRapportApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<DataContext>(options =>
{
    options.EnableSensitiveDataLogging(false);
    options.UseSqlServer("Server=tcp:bgserverinst.database.windows.net,1433;Initial Catalog=inst-db-report;Persist Security Info=False;User ID=andreasbgjensen;Password=Firma2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", options => options.EnableRetryOnFailure().CommandTimeout(60));

});
builder.Services.AddTransient<DataAccess.Interface.IInstitutionTableRepository, InstitutionTabelReporisory>();
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
