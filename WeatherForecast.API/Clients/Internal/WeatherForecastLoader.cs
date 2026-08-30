using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WeatherForecast.Abstractions;
using WeatherForecast.API.Options;
using WeatherForecast.Models;

namespace WeatherForecast.API.Internal;

public class WeatherForecastLoader(IOptions<WeatherForecastDefaults> defaults, IWeatherAPI weatherAPI)
    : IWeatherForecastLoader
{
    private readonly WeatherForecastDefaults _defaults = defaults.Value;

    public async Task<Models.WeatherForecastEntry[]> LoadAsync(float latitude, float longitude, int days,
        CancellationToken cancellationToken)
    {
        var queryParamsBuilder = new QueryBuilder([
            new KeyValuePair<string, string>("key", _defaults.ApiKey),
            new KeyValuePair<string, string>("q", new StringValues([latitude.ToString(), longitude.ToString()])),
            new KeyValuePair<string, string>("days", days.ToString())
        ]);
        var response = await weatherAPI.GetWeatherForecast(new WeatherAPIQueryParams
        {
            Key = defaults.Value.ApiKey,
            Days = days.ToString(),
            Query = queryParamsBuilder.ToString()
        });

        foreach (var forecastDay in response.Forecast.Days)
        {
            forecastDay.Date = DateTime.SpecifyKind(forecastDay.Date, DateTimeKind.Utc);
        }

        return response.Forecast.Days
            .Select(x => new Models.WeatherForecastEntry(
                x.Day.MaxTempC,
                x.Day.MinTempC,
                x.Date,
                x.Hours.Select(h =>
                {
                    var parsedDate = DateTime.ParseExact(h.Time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    return new Models.WeatherForecastHourly()
                    {
                        TempC = h.TempC,
                        Time = parsedDate.TimeOfDay
                    };
                }).ToArray()))
            .ToArray();
    }
}