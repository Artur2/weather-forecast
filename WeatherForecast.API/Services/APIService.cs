
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using WeatherForecast.API;
using WeatherForecast.Commands;

namespace WeatherForecast.API.Services;

public class APIService(IMediator mediator) : ApiService.ApiServiceBase
{
    public override async Task<WeatherForecastResponse> GetWeatherForecast(WeatherForecastRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetWeatherForecastCommand(request.Latitude, request.Longitued, request.Days));
        return new WeatherForecastResponse
        {
            Entries = {
                result.Select(x => new WeatherForecastEntryResponse
                {
                    MaxTempC = x.MaxTempC,
                    MinTempC = x.MinTempC,
                    Hours = { x.Hourly.Select(h => new WeatherForecastHourlyResponse
                        {
                            TempC = h.TempC,
                            Time = h.Time.ToDuration()
                        })
                    },
                    Date = x.Date.ToTimestamp()
                })
            }
        };
    }
}