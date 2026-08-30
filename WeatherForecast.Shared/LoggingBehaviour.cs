using Microsoft.Extensions.Logging;
using MediatR;

namespace WeatherForecast.Shared;

public class LoggingBehaviour<TRequest, TResponse>(ILoggerFactory loggerFactory) : IPipelineBehavior<TRequest, TResponse>
where TRequest : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(nameof(LoggingBehaviour<,>));
        logger.LogInformation("Started request processing");
        try
        {
            return next();
        }
        finally
        {
            logger.LogInformation("Processed request");
        }
    }
}
