
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using WeatherForecast.API;

public class APIClientFactory
{
    private readonly IOptions<GrpcOptions> _options;

    public APIClientFactory()
    {
    }
    
    public APIClientFactory(IOptions<GrpcOptions> options)
    {
        _options = options;
    }
    
    public virtual ApiService.ApiServiceClient Create()
    {
        var host = _options.Value.ApiHost;
        var channel = GrpcChannel.ForAddress(host);
        var client = new ApiService.ApiServiceClient(channel);

        return client;
    }
}