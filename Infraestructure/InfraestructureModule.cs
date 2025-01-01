using Core.Repositories;
using Infraestructure.Messaging;
using Infraestructure.MongoUtil;
using Infraestructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infraestructure
{
    public static class InfraestructureModule
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {
            services
                .AddMongo()
                .AddMessageBus()
                .AddRepositories();

            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbOptions>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = new MongoDbOptions();

                configuration.GetSection("Mongo").Bind(options);

                return options;
            });

            services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = sp.GetService<MongoDbOptions>();

                var client = new MongoClient(options.ConnectionString);
                var db = client.GetDatabase(options.Database);

                return client;
            });

            services.AddTransient(sp => {
                BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

                var options = sp.GetService<MongoDbOptions>();
                var mongoClient = sp.GetService<IMongoClient>();

                var db = mongoClient.GetDatabase(options.Database);

                return db;
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        private static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddScoped<IMessageBusService, RabbitMqService>();

            return services;
        }
    }
}
