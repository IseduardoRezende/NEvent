using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace NEvent.Log
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNEventLogging(
            this IServiceCollection services,
            Action<ILoggingBuilder>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            return services.AddLogging(builder =>
            {                
                builder.SetMinimumLevel(LogLevel.Information);
                configure?.Invoke(builder);
            });
        }
    }
}
