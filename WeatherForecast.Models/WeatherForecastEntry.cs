namespace WeatherForecast.Models;

public record WeatherForecastEntry(float MaxTempC, float MinTempC, DateTime Date, WeatherForecastHourly[] Hourly);