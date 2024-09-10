using FlightAnalyzerAPI.Helpers;
using FlightAnalyzerAPI.Interface;
using FlightAnalyzerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register the FileReader service
builder.Services.AddSingleton<IFileReader, FileReader>();

// Retrieve the file path from configuration
var csvFilePath = builder.Configuration.GetSection("FlightData")["CsvFilePath"];
var fullPath = Path.Combine(Directory.GetCurrentDirectory(), csvFilePath);

// Register the FlightService with the CSV file path and FileReader dependency
builder.Services.AddSingleton<IFlightService>(provider =>
    new FlightService(fullPath, provider.GetRequiredService<IFileReader>())
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
