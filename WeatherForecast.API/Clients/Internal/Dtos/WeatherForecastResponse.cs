using System.Text.Json.Serialization;

namespace WeatherForecast.API.Internal.Dtos;

// Лучше конечно automapper для всего этого дела
public class WeatherForecastResponse
{
    [JsonPropertyName("forecast")] public Forecast Forecast { get; set; }
}

public class Forecast
{
    [JsonPropertyName("forecastday")] public ForecastDay[] Days { get; set; }
}

public class ForecastDay
{
    [JsonPropertyName("day")] public Day Day { get; set; }

    [JsonPropertyName("hour")] public Hour[] Hours { get; set; }
    
    [JsonPropertyName("date")] public DateTime Date { get; set; }
}

public class Day
{
    [JsonPropertyName("maxtemp_c")] public float MaxTempC { get; set; }

    [JsonPropertyName("mintemp_c")] public float MinTempC { get; set; }
}

public class Hour
{
    [JsonPropertyName("time")] public string Time { get; set; }

    [JsonPropertyName("temp_c")] public float TempC { get; set; }
}