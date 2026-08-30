namespace WeatherForecast.Models;

public struct WeatherForecastHourly
{
    public TimeSpan Time { get; init; }
    
    public float TempC { get; init; }
}