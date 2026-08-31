using Grpc.Core;
using Grpc.Core.Interceptors;
using WeatherForecast.API.RateLimiting;

namespace WeatherForecast.API.Interceptors;

public class RateLimitingInterceptor(RateLimitingFactory rateLimitingFactory) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var rateLimiter = await rateLimitingFactory.CreateAsync();

        using var lease = await rateLimiter.AcquireAsync(cancellationToken: context.CancellationToken);
        if (lease.IsAcquired)
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }

        throw CreateException(StatusCode.ResourceExhausted, "ResourceExhausted");
    }

    private RpcException CreateException(StatusCode statusCode, string message) => new(new Status(statusCode, message));
}