using Ecommerce.Domain.Core.Interfaces.Repositories;
using Ecommerce.Domain.Core.Interfaces.Services;
using Ecommerce.Infrastructure.CrossCutting.Options;
using Ecommerce.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;

namespace Ecommerce.Infrastructure.CrossCutting.Extensions.Ioc
{
    public static class ServicesCollectionExtensions
    {
        public  static IServiceCollection AddRavenDb(this IServiceCollection servicesCollection)
        {
            
            servicesCollection.TryAddSingleton<IDocumentStore>(ctx => 
            {
                var ravenDbSettings = ctx.GetRequiredService<IOptions<RavenDbSettings>>().Value;
                var store = new DocumentStore
                {
                    Urls = new[] { ravenDbSettings.Url },
                    Database = ravenDbSettings.DataBaseName
                };

                store.Initialize();

                return store;
            });

            return servicesCollection;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection servicesCollection)
        {
            servicesCollection.TryAddSingleton<ICustomerRepository, CustomerRepository>();
            return servicesCollection;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection servicesCollection)
        {
            servicesCollection.TryAddScoped<ICustomerService, CustomerService>();
            return servicesCollection;
        }
    }
}
