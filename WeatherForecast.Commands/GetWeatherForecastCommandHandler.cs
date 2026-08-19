using MediatR;
using WeatherForecast.Abstractions;
using WeatherForecast.Models;

namespace WeatherForecast.Commands;

public class GetWeatherForecastCommandHandler(IWeatherForecastLoader weatherForecastLoader) : IRequestHandler<GetWeatherForecastCommand, WeatherForecastEntry[]>
{
    public Task<WeatherForecastEntry[]> Handle(GetWeatherForecastCommand request, CancellationToken cancellationToken)
    {
        return weatherForecastLoader.LoadAsync(request.Latitude, request.Longitude, request.Days, cancellationToken);
    }
}