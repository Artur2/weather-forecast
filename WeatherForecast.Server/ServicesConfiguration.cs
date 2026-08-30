using WeatherForecast.Abstractions;
using WeatherForecast.Commands;
using WeatherForecast.Models;
using WeatherForecast.Server.Internal;
using WeatherForecast.Shared;

namespace WeatherForecast.Server;

public class ServicesConfiguration
{
    public static void ConfigureServices(IServiceCollection services)
    {
        /* Можно использовать user_secrets или vault в случаях с распределенными настройками */
        services.AddTransient<IWeatherForecastLoader, WeatherForecastLoader>();
        services.AddMediatR((cfg) =>
            cfg.RegisterServicesFromAssembly(typeof(GetWeatherForecastCommand).Assembly)
            .AddBehavior<LoggingBehaviour<GetWeatherForecastCommand, WeatherForecastEntry[]>>());
    }
}