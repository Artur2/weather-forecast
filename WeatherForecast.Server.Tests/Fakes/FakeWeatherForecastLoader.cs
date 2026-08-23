using WeatherForecast.Abstractions;
using WeatherForecast.Models;

namespace WeatherForecast.Server.Tests.Fakes;

public class FakeWeatherForecastLoader : IWeatherForecastLoader
{
    public Task<WeatherForecastEntry[]> LoadAsync(float latitude, float longitude, int days,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new[]
        {
            new WeatherForecastEntry(1, 2, DateTime.UtcNow, Enumerable.Range(0, 23)
                .Select(x => new WeatherForecastHourly()
                {
                    TempC = x, Time = DateTime.UtcNow.Date.AddHours(x)
                })
                .ToArray()),
            new WeatherForecastEntry(1, 2, DateTime.UtcNow.AddDays(1), Enumerable.Range(0, 23)
                .Select(x => new WeatherForecastHourly()
                {
                    TempC = x, Time = DateTime.UtcNow.AddDays(1).Date.AddHours(x)
                })
                .ToArray()),
            new WeatherForecastEntry(1, 2, DateTime.UtcNow.AddDays(2), Enumerable.Range(0, 23)
                .Select(x => new WeatherForecastHourly()
                {
                    TempC = x, Time = DateTime.UtcNow.AddDays(2).Date.AddHours(x)
                })
                .ToArray()),
        });
    }
}