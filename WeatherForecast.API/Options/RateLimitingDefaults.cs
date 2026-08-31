namespace WeatherForecast.API.Options;

public class RateLimitingDefaults
{
    public int QueueLimit { get; set; }
    public int PermitLimit { get; set; }
    public TimeSpan Window { get; set; }
}