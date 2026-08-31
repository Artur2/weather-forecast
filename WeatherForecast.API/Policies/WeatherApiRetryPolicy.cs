using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using WeatherForecast.API.Options;

namespace WeatherForecast.API.Policies;

public class WeatherApiRetryPolicy(IOptions<WeatherRetryPolicyDefaults> options)
{
    public AsyncRetryPolicy Create()
    {
        return Policy.Handle<Exception>()
            .WaitAndRetryAsync(options.Value.RetryAttempts,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(options.Value.Multiplication, retryAttempt)));
    }
}