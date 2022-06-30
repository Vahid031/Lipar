using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Market.Infrastructure.Data.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Market.Presentation.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        //Read Configuration from appSettings
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.Serilog.json")
        .Build();
        //Initialize Logger
        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .CreateLogger();
        
        
        var host = CreateHostBuilder(args).Build();
        
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            
            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                
                await DefaultRoles.SeedAsync(userManager, roleManager);
                await DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                
                Log.Information("Finished Seeding Default Data");
                
                host.Run();
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
        
    }
    
    public static IHostBuilder CreateHostBuilder(string[] args)
    => Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>()
        .CaptureStartupErrors(true)
        .ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Serilog.json", optional: true)
            .AddJsonFile("appsettings.Lipar.json", optional: true);
        })
        .UseSerilog();
    });
}


