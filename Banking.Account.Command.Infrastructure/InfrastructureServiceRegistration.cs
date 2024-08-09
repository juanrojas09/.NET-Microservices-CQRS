using Banking.Account.Command.Application.Aggregates;
using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Infrastructure.KafkaEvents;
using Banking.Account.Command.Infrastructure.Repositories;
using Banking.Cqrs.Core.Events;
using Banking.Cqrs.Core.Handlers;
using Banking.Cqrs.Core.Infrastructure;
using Banking.Cqrs.Core.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        /// <summary>
        /// Inyeccion de dependencias para la infraestructura
        /// explicacion: 
        /// sirve para implementar la inyeccion de los servicios de infra con las diferentes clases que
        /// implementan concretamente las interfaces
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //configuracion para q mongo sepa como serializar los eventos
            // y adaptar los diferentes tipos de eventos

            BsonClassMap.RegisterClassMap<BaseEvent>();
            BsonClassMap.RegisterClassMap<AccountOpenedEvent>();
            BsonClassMap.RegisterClassMap<AccountClosedEvent>();
            BsonClassMap.RegisterClassMap<FundsDepositedEvent>();
            BsonClassMap.RegisterClassMap<FundsWithdrawnEvent>();

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddScoped<IEventProducer, AccountEventProducer>();
            services.AddTransient<IEventStoreRepository, EventStoreRepository>();

            services.AddTransient<IEventStore, AccountEventStore>();
            services.AddTransient<IEventSourcingHandler<AccountAggregate>,AccountEventSourcingHandler>();


            return services;
        }
    }
}
