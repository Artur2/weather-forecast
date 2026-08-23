using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Abstractions;
using WeatherForecast.Server.Tests.Fakes;

namespace WeatherForecast.Server.Tests;

public class TestsServicesConfiguration
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IWeatherForecastLoader, FakeWeatherForecastLoader>();
    }
}