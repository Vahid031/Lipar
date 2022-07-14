using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;
using Market.Infrastructure.Data.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.Extensions.DependencyInjection;
using Market.Presentation.Api;




//Read Configuration from appSettings
var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.Serilog.json")
.Build();
//Initialize Logger
Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .CreateLogger();


var app = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>()
        .CaptureStartupErrors(true)
        .ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Serilog.json", optional: true)
            .AddJsonFile("appsettings.Lipar.json", optional: true);
        });
    }).Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultSuperAdmin.SeedAsync(userManager, roleManager);

        Log.Information("Finished Seeding Default Data");

        app.Run();
        Log.Information("Application Starting");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Application start-up failed");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

