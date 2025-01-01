using Application.ListenerRabbitMQ;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddSubscribers()
                .AddApplicationServices();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }

        private static IServiceCollection AddSubscribers(this IServiceCollection services)
        {
            services.AddHostedService<OrderCreatedListener>();

            return services;
        }
    }
}