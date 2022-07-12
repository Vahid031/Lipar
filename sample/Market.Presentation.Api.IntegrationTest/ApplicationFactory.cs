using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Market.Infrastructure.Data.SqlServer.Commands.Common;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using Market.Infrastructure.Data.Identity.Contexts;

namespace Market.Presentation.Api.IntegrationTest;

public class ApplicationFactory : WebApplicationFactory<Startup>
{
    protected override IHostBuilder CreateHostBuilder()
    {
        var host = Host.CreateDefaultBuilder()
                .ConfigureWebHost(builder =>
                {
                    builder.UseStartup<Startup>();

                })
                .ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile("appsettings.json", optional: true)
                          .AddJsonFile("appsettings.Serilog.json", optional: true)
                          .AddJsonFile("appsettings.Lipar.json", optional: true);
                });

        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.InMemoryDBContext<MarketQueryDbContext>("TestDb")
                    .InMemoryDBContext<MarketCommandDbContext>("TestDb")
                    .InMemoryDBContext<IdentityContext>("TestDb");

        });


    }   
}
