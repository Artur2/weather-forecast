namespace WeatherForecast.API.Options;

public class WeatherRetryPolicyDefaults
{
    public int RetryAttempts { get; set; }
    
    public int Multiplication { get; set; }
}