using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lipar.Presentation.Api.Extensions;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using Microsoft.EntityFrameworkCore;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Market.Infrastructure.Data.Identity.Contexts;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Market.Infrastructure.Data.SqlServer.Commands.Common;

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

            //services.AddScoped<MarketCommandDbContext>();

            services.AddDbContext<MarketQueryDbContext>(
                c => c.UseSqlServer(Configuration.GetConnectionString("QueryConnectionString")));

            services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("IdentityConnectionString"),
                        b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

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
                    //options.RequireHttpsMetadata = false;
                    //options.SaveToken = false;
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
                    //options.Events = new JwtBearerEvents()
                    //{
                    //    OnAuthenticationFailed = c =>
                    //    {
                    //        c.NoResult();
                    //        c.Response.StatusCode = 500;
                    //        c.Response.ContentType = "text/plain";
                    //        return c.Response.WriteAsync(c.Exception.ToString());
                    //    },
                    //    OnChallenge = context =>
                    //    {
                    //        context.HandleResponse();
                    //        context.Response.StatusCode = 401;
                    //        context.Response.ContentType = "application/json";
                    //        var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                    //        return context.Response.WriteAsync(result);
                    //    },
                    //    OnForbidden = context =>
                    //    {
                    //        context.Response.StatusCode = 403;
                    //        context.Response.ContentType = "application/json";
                    //        var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                    //        return context.Response.WriteAsync(result);
                    //    },
                    //};
                });

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
