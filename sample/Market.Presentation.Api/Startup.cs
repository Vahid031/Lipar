using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Lipar.Presentation.Api.Extensions;
using Market.Infrastructure.Data.SqlServerCommand.Common;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using Microsoft.EntityFrameworkCore;

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



            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Market.Presentation.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Market.Presentation.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
