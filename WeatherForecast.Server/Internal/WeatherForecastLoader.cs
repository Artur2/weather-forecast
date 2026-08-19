using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WeatherForecast.Abstractions;
using WeatherForecast.Models;
using WeatherForecast.Server.Internal.Dtos;
using WeatherForecast.Server.Options;

namespace WeatherForecast.Server.Internal;

public class WeatherForecastLoader(IOptions<WeatherForecastDefaults> defaults, IHttpClientFactory httpClientFactory)
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
        var uriBuilder = new UriBuilder
        {
            Host = _defaults.Host,
            Path = _defaults.Path,
            Query = queryParamsBuilder.ToString()
        };

        using var client = httpClientFactory.CreateClient();
        var response =
            await client.GetFromJsonAsync<WeatherForecastResponse>(uriBuilder.ToString(),
                cancellationToken: cancellationToken);

        if (response == null)
        {
            throw new InvalidOperationException("Unable to load weather forecast entry");
        }

        return response.Forecast.Days
            .Select(x => new WeatherForecastEntry(
                x.Day.MaxTempC,
                x.Day.MinTempC,
                x.Date,
                x.Hours.Select(h =>
                    new WeatherForecastHourly()
                    {
                        TempC = h.TempC,
                        Time = DateTime.ParseExact(h.Time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                    }).ToArray()))
            .ToArray();
    }
}