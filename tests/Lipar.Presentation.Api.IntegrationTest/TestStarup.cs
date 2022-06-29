using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.AspNetCore.Builder;
using Lipar.Presentation.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Market.Infrastructure.Data.Identity.Models;
using Market.Infrastructure.Data.SqlServer.Commands.Common;
using Market.Infrastructure.Data.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lipar.Presentation.Api.IntegrationTest
{
    public class TestStarup
    {
        public TestStarup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLiparServices(Configuration, nameof(Lipar), nameof(Market));

            services.AddDbContext<MarketCommandDbContext>(
                c => c.UseInMemoryDatabase("MarketDb"));

            services.AddDbContext<MarketQueryDbContext>(
                c => c.UseInMemoryDatabase("MarketDb"));

            services.AddDbContext<IdentityContext>(options =>
                   options.UseInMemoryDatabase("IdentityDb"));


            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

            services.Configure<JWTSetting>(Configuration.GetSection("JWTSetting"));

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JWTSetting:Issuer"],
                        ValidAudience = Configuration["JWTSetting:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSetting:Key"]))
                    };
                });

            services.AddCors(setupAction =>
           setupAction.AddPolicy("MyPolicy",
                             builder => builder
                             .AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader()
                             ));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LiparOptions liparOptions)
        {
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.AddLiparConfiguration(env, liparOptions);
        }
    }
}
