using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;

namespace Market.Presentation.Api.IntegrationTest;

public static class ApplicationFactoryExtension
{
    public static IServiceCollection InMemoryDBContext<TDbContext>(this IServiceCollection services, string databaseName) where TDbContext : DbContext
    {
        ServiceDescriptor descriptor = services.Single(
                   d => d.ServiceType == typeof(DbContextOptions<TDbContext>));
        if (descriptor != null) services.Remove(descriptor);

        services.AddDbContext<TDbContext>(o =>
            o.UseInMemoryDatabase(databaseName));

        return services;
    }


    public static HttpContent ToStringContent<TEntity>(this TEntity entity) where TEntity : class 
    {
        string payload = JsonConvert.SerializeObject(entity);

        return new StringContent(payload, Encoding.UTF8, "application/json");
    }

}