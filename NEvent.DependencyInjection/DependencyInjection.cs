using NEvent.Core;
using NEvent.Interfaces;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NEvent.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNEvent(this IServiceCollection services, params Assembly[] eventAssemblies)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(eventAssemblies, nameof(eventAssemblies));

            return services
                .AddSingleton(typeof(ISubscriber<>), typeof(Subscriber<>))
                .AddScoped<IEventAggregator, EventAggregator>()
                .AddScoped<ISubscriberProvider, SubscriberProvider>()
                .AddScoped<IEventFilterProvider, EventFilterProvider>()
                .AddNEventInterfaces(eventAssemblies);
        }

        private static IServiceCollection AddNEventInterfaces(this IServiceCollection services, params Assembly[] eventAssemblies)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(eventAssemblies, nameof(eventAssemblies));

            foreach (var eventAssembly in eventAssemblies)
            {
                List<Type> types =
                [..
                    eventAssembly.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false })
                ];

                foreach (Type type in types)
                {
                    List<Type> interfaces =
                    [..
                        type.GetInterfaces().Where(i => i.IsGenericType &&
                                                       (i.GetGenericTypeDefinition() == typeof(IEventHandler<>) ||
                                                        i.GetGenericTypeDefinition() == typeof(IEventFilter<>)))
                    ];

                    foreach (var @interface in interfaces)
                    {
                        services.AddScoped(@interface, type);
                    }
                }
            }

            return services;
        }
    }
}
