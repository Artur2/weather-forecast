using Microsoft.AspNetCore.Server.Kestrel.Core;
using WeatherForecast.API;
using WeatherForecast.API.Options;
using WeatherForecast.API.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Configuration.AddJsonFile("weather_forecast.json");
        builder.Services.Configure<WeatherForecastDefaults>(builder.Configuration.GetSection("weather_forecast"));
        ServicesConfiguration.ConfigureServices(builder.Services);
    
        var app = builder.Build();

        app.MapGrpcService<APIService>();
        app.MapGrpcReflectionService();

        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

        app.Run();
    }
}