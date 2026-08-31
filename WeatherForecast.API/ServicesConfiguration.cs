using WeatherForecast.Abstractions;
using WeatherForecast.API.Internal;
using WeatherForecast.Models;
using WeatherForecast.Commands;
using WeatherForecast.Shared;
using WeatherForecast.API.Options;
using Refit;
using Microsoft.Extensions.Options;
using WeatherForecast.API.Interceptors;
using WeatherForecast.API.Policies;
using WeatherForecast.API.RateLimiting;

namespace WeatherForecast.API;

public class ServicesConfiguration
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddRefitClient<IWeatherAPI>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<WeatherForecastDefaults>>();
                var host = options.Value.Host;
                client.BaseAddress = new Uri(host);
            });
        services.AddGrpc(options => { options.Interceptors.Add<RateLimitingInterceptor>(); });
        services.AddGrpcReflection();

        /* Можно использовать user_secrets или vault в случаях с распределенными настройками */
        services.AddTransient<IWeatherForecastLoader, WeatherForecastLoader>();
        services.AddSingleton<RateLimitingFactory>();
        services.AddTransient<WeatherApiRetryPolicy>();
        services.AddMediatR((cfg) =>
            cfg.RegisterServicesFromAssembly(typeof(GetWeatherForecastCommand).Assembly)
                .AddBehavior<LoggingBehaviour<GetWeatherForecastCommand, WeatherForecastEntry[]>>());
    }
}