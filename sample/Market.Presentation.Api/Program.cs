using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Market.Presentation.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Serilog.json")
                .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
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
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            //loggerConfiguration
                            //    .ReadFrom.Configuration(hostingContext.Configuration)
                            //    .Enrich.FromLogContext()
                            //    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                            //    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

                            //#if DEBUG
                            // Used to filter out potentially bad data due debugging.
                            // Very useful when doing Seq dashboards and want to remove logs under debugging session.
                            //loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
                            //#endif
                        });
                   });
    }
}
