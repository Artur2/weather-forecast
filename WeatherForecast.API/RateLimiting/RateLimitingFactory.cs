using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using WeatherForecast.API.Options;

namespace WeatherForecast.API.RateLimiting;

public class RateLimitingFactory(IOptions<RateLimitingDefaults> options) : IDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    private readonly Lazy<FixedWindowRateLimiter> _fixedWindowRateLimiter = new(() =>
        new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions()
        {
            QueueLimit = options.Value.QueueLimit,
            PermitLimit = options.Value.PermitLimit,
            Window = options.Value.Window
        }));

    public async ValueTask<FixedWindowRateLimiter> CreateAsync()
    {
        if (_fixedWindowRateLimiter.IsValueCreated)
        {
            return _fixedWindowRateLimiter.Value;
        }

        await _semaphoreSlim.WaitAsync();
        try
        {
            return _fixedWindowRateLimiter.Value;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public void Dispose()
    {
        _semaphoreSlim.Dispose();
    }
}