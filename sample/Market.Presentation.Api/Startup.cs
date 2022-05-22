using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lipar.Presentation.Api.Extensions;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data.SqlServer.Commands.Common;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Infrastructure.Identity;

namespace Market.Presentation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLiparServices(Configuration, nameof(Lipar), nameof(Market));

            services.AddDbContext<MarketCommandDbContext>(
                c => c.UseSqlServer(Configuration.GetConnectionString("CommandConnectionString")));

            services.AddDbContext<MarketQueryDbContext>(
                c => c.UseSqlServer(Configuration.GetConnectionString("QueryConnectionString")));



            services.AddIdentityInfrastructure(Configuration);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LiparOptions liparOptions)
        {
            app.AddLiparConfiguration(env, liparOptions);
        }
    }
}
