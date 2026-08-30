
using Refit;
using WeatherForecast.API.Internal.Dtos;

public class WeatherAPIQueryParams
{
    [AliasAs("key")]
    public string Key { get; set; }

    [AliasAs("q")]
    public string Query { get; set; }

    [AliasAs("days")]
    public string Days { get; set; }
}

public interface IWeatherAPI
{
    [Get("/v1/forecast.json")]
    Task<WeatherForecastResponse> GetWeatherForecast([Query] WeatherAPIQueryParams @params);
}