using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeatherForecast.API;

namespace WeatherForecast.Server.Tests;

public class Mocks
{
    public static void ConfigureClientMock(IServiceCollection serviceCollection)
    {
        var clientMock = new Mock<ApiService.ApiServiceClient>();
        var clientFactoryMock = new Mock<APIClientFactory>();
        clientFactoryMock.Setup(x => x.Create())
            .Returns(clientMock.Object);
        
        var call = TestCalls.AsyncUnaryCall(responseAsync: Task.FromResult(new WeatherForecastResponse
        {
            Entries =
            {
                {
                    [
                        new WeatherForecastEntryResponse
                        {
                            MaxTempC = 40,
                            MinTempC = 25,
                            Hours =
                            {
                                Enumerable.Range(0, 23).Select(x => new WeatherForecastHourlyResponse()
                                {
                                    TempC = x + 9, Time = DateTime.UtcNow.AddDays(1).Date.AddHours(x).TimeOfDay.ToDuration()
                                })
                            },
                            Date = DateTime.UtcNow.ToTimestamp()
                        },
                        new WeatherForecastEntryResponse()
                        {
                            MaxTempC = 40,
                            MinTempC = 25,
                            Hours =
                            {
                                Enumerable.Range(0, 23).Select(x => new WeatherForecastHourlyResponse()
                                {
                                    TempC = x + 10, Time = DateTime.UtcNow.AddDays(1).Date.AddHours(x).TimeOfDay.ToDuration()
                                })
                            },
                            Date = DateTime.UtcNow.AddDays(1).ToTimestamp()
                        },
                        new WeatherForecastEntryResponse()
                        {
                            MaxTempC = 40,
                            MinTempC = 25,
                            Hours =
                            {
                                Enumerable.Range(0, 23).Select(x => new WeatherForecastHourlyResponse()
                                {
                                    TempC = x + 11, Time = DateTime.UtcNow.AddDays(1).Date.AddHours(x).TimeOfDay.ToDuration()
                                })
                            },
                            Date = DateTime.UtcNow.AddDays(2).ToTimestamp()
                        }
                    ]
                }
            }
        }), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });

        clientMock.Setup(x => x.GetWeatherForecastAsync(It.IsAny<WeatherForecastRequest>(), It.IsAny<Metadata>(),
                It.IsAny<DateTime?>()
                , It.IsAny<CancellationToken>()))
            .Returns(call);
        
        serviceCollection.AddSingleton(clientMock.Object);
        serviceCollection.AddSingleton(clientFactoryMock.Object);
    }
}