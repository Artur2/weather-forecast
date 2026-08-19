namespace WeatherForecast.Models;

public struct WeatherForecastHourly
{
    public DateTime Time { get; init; }
    
    public float TempC { get; init; }
}