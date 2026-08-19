using MediatR;
using WeatherForecast.Models;

namespace WeatherForecast.Commands;

public record GetWeatherForecastCommand(float Latitude, float Longitude, int Days) : IRequest<WeatherForecastEntry[]>;