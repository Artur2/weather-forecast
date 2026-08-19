using WeatherForecast.Models;

namespace WeatherForecast.Abstractions;

public interface IWeatherForecastLoader
{
    Task<WeatherForecastEntry[]> LoadAsync(float latitude, float longitude, int days,
        CancellationToken cancellationToken);
}