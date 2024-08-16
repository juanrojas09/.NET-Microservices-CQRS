using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using Banking.Account.Query.Infrastructure.Persistence;
using Banking.Account.Query.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {

        /// <summary>
        /// Inyeccion de dependencias para la infraestructura
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration
            )
        {
            var connectionString=configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<MySqlDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
           
            //general
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            //especifico
           // services.AddScoped<IAsyncRepository<BankAccount>, BankAccountRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();



            return services;
        }

    }
}
