using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Lipar.Presentation.Api.Extensions;
using Market.Infrastructure.Data.SqlServer.Commands.Common;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data.Identity.Contexts;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Linq;
using Lipar.Core.Contract.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);


builder.Services.AddLiparServices(builder.Configuration, nameof(Lipar), nameof(Market));

builder.Services.AddDbContext<MarketCommandDbContext>(
c => c.UseSqlServer(builder.Configuration.GetConnectionString("CommandConnectionString")));

//services.AddScoped<MarketCommandDbContext>();

builder.Services.AddDbContext<MarketQueryDbContext>(
c => c.UseSqlServer(builder.Configuration.GetConnectionString("QueryConnectionString")));

builder.Services.AddDbContext<IdentityContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnectionString")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();


builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JWTSetting"));

builder.Services.AddAuthentication(o =>
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
        ValidIssuer = builder.Configuration["JWTSetting:Issuer"],
        ValidAudience = builder.Configuration["JWTSetting:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:Key"]))
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

builder.Services.AddCors(setupAction =>
setupAction.AddPolicy("MyPolicy",
builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
));

builder.Services.AddSwaggerGen(c =>
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


var app = builder.Build();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
var liparOptions = app.Services.GetRequiredService<LiparOptions>();

app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.AddLiparConfiguration(env, liparOptions);

app.Run();

//if (env.IsDevelopment())
//{
//    using (var scope = app.ApplicationServices.CreateScope())
//    {
//        var commandDb = scope.ServiceProvider.GetService<MarketCommandDbContext>();
//        var identityDb = scope.ServiceProvider.GetService<IdentityContext>();

//        commandDb.Database.EnsureCreated();
//        identityDb.Database.EnsureCreated();
//    }
//}



////Read Configuration from appSettings
//var config = new ConfigurationBuilder()
//.AddJsonFile("appsettings.Serilog.json")
//.Build();
////Initialize Logger
//Log.Logger = new LoggerConfiguration()
//        .ReadFrom.Configuration(config)
//        .CreateLogger();




//var app = Host.CreateDefaultBuilder(args)
//    .ConfigureWebHostDefaults(webBuilder =>
//    {
//        webBuilder.UseStartup<Startup>()
//        .CaptureStartupErrors(true)
//        .ConfigureAppConfiguration(config =>
//        {
//            config.AddJsonFile("appsettings.json", optional: true)
//            .AddJsonFile("appsettings.Serilog.json", optional: true)
//            .AddJsonFile("appsettings.Lipar.json", optional: true);
//        });
//    }).Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    try
//    {
//        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
//        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//        await DefaultRoles.SeedAsync(userManager, roleManager);
//        await DefaultSuperAdmin.SeedAsync(userManager, roleManager);

//        Log.Information("Finished Seeding Default Data");

//        app.Run();
//        Log.Information("Application Starting");
//    }
//    catch (Exception ex)
//    {
//        Log.Fatal(ex, "Application start-up failed");
//    }
//    finally
//    {
//        Log.CloseAndFlush();
//    }
//}

