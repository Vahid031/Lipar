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
using System;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

            services.AddCors(setupAction =>
           setupAction.AddPolicy("MyPolicy",
                             builder => builder
                             .AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader()
                             ));

            services.AddSwaggerGen(c =>
            {

                //c.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Version = "v1",
                //    Title = "Market v1.0",
                //    Description = "This Api will be responsible for overall data distribution and authorization.",
                //    Contact = new OpenApiContact
                //    {
                //        Name = "Vahid Goodarzi",
                //        Email = "vahid031@yahoo.com",
                //        Url = new Uri("http://Vahid031.ir"),
                //    }
                //});


                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };
                c.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, Array.Empty<string>());
                c.AddSecurityRequirement(securityRequirement);
            });
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
