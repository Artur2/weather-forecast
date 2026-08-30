
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using WeatherForecast.API;

public class APIClientFactory(IOptions<GrpcOptions> options)
{
    public ApiService.ApiServiceClient Create()
    {
        var host = options.Value.ApiHost;
        var channel = GrpcChannel.ForAddress(host);
        var client = new ApiService.ApiServiceClient(channel);

        return client;
    }
}