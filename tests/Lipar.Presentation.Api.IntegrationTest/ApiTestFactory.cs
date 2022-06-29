using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Lipar.Presentation.Api.IntegrationTest
{
    public class ApiTestFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                })
                .ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile("appsettings.json", optional: true)
                         .AddJsonFile("appsettings.Lipar.json", optional: true);
                }); ;

            return host;
        }
    }
}
