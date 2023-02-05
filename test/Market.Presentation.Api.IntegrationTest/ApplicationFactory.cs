using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Market.Infrastructure.Data.Identity.Contexts;
using Lipar.Core.Contract.Data;
using Market.Presentation.Api.IntegrationTest.TestRepositories;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;

namespace Market.Presentation.Api.IntegrationTest;

public class ApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //services.InMemoryDBContext<SqlServerMarketQueryDbContext>("TestDb")
            //        .InMemoryDBContext<SqlServerMarketCommandDbContext>("TestDb")
            //        .InMemoryDBContext<IdentityContext>("TestDb");


            services.AlterService<IOutBoxEventRepository, TestOutBoxEventRepository>();
            services.AlterService<IInBoxEventRepository, TestInBoxEventRepository>();
            services.AlterService<IEntityChangesInterceptorRepository, TestEntityChangesInterceptorRepository>();
            services.AlterService<IEventBus, TestEventBus>();
            services.AlterService<ITranslator, TestTranslator>();

        });


    }
}
