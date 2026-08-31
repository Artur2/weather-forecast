using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WeatherForecast.Abstractions;
using WeatherForecast.API.Options;
using WeatherForecast.API.Policies;
using WeatherForecast.Models;

namespace WeatherForecast.API.Internal;

public class WeatherForecastLoader(
    IOptions<WeatherForecastDefaults> defaults,
    IWeatherAPI weatherAPI,
    WeatherApiRetryPolicy retryPolicy)
    : IWeatherForecastLoader
{
    private readonly WeatherForecastDefaults _defaults = defaults.Value;

    public async Task<WeatherForecastEntry[]> LoadAsync(float latitude, float longitude, int days,
        CancellationToken cancellationToken)
    {
        var queryParamsBuilder = new QueryBuilder([
            new KeyValuePair<string, string>("key", _defaults.ApiKey),
            new KeyValuePair<string, string>("q", new StringValues([latitude.ToString(), longitude.ToString()])),
            new KeyValuePair<string, string>("days", days.ToString())
        ]);

        var policy = retryPolicy.Create();

        var response = await policy.ExecuteAsync(async () => await weatherAPI.GetWeatherForecast(
            new WeatherAPIQueryParams
            {
                Key = defaults.Value.ApiKey,
                Days = days.ToString(),
                Query = queryParamsBuilder.ToString()
            }));

        foreach (var forecastDay in response.Forecast.Days)
        {
            // Google требует использование дат в UTC
            forecastDay.Date = DateTime.SpecifyKind(forecastDay.Date, DateTimeKind.Utc);
        }

        return response.Forecast.Days
            .Select(x => new WeatherForecastEntry(
                x.Day.MaxTempC,
                x.Day.MinTempC,
                x.Date,
                x.Hours.Select(h =>
                {
                    var parsedDate = DateTime.ParseExact(h.Time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    return new WeatherForecastHourly()
                    {
                        TempC = h.TempC,
                        Time = parsedDate.TimeOfDay
                    };
                }).ToArray()))
            .ToArray();
    }
}