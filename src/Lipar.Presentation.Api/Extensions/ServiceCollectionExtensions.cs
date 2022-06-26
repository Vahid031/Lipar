using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using FluentValidation.AspNetCore;
using Lipar.Core.Application.Services;
using Lipar.Infrastructure.Tools.IoC;

namespace Lipar.Presentation.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLiparServices(this IServiceCollection services,
           IConfiguration configuration,
           params string[] assemblyNames)
        {
            var liparOptions = new LiparOptions();
            configuration.GetSection(nameof(LiparOptions)).Bind(liparOptions);
            services.AddSingleton(liparOptions);

            var assembies = GetAssemblies(assemblyNames);

            services.AddApplication(assembies);

            //services.AddHostedService<PoolingPublisherHostedService>();

            services.AddControllers(
                options =>
                {
                    //options.Filters.Add(typeof(TrackActionPerformanceFilter));
                }).AddFluentValidation();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(liparOptions.Swagger.Version,
                    new OpenApiInfo
                    {
                        Title = liparOptions.Swagger.Title,
                        Version = liparOptions.Swagger.Version
                    });
            });
        }

        private static List<Assembly> GetAssemblies(string[] assmblyName)
        {

            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateCompilationLibrary(library, assmblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }

        private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary, string[] assmblyName)
        {
            return assmblyName.Any(d => compilationLibrary.Name.Contains(d))
                || compilationLibrary.Dependencies.Any(d => assmblyName.Any(c => d.Name.Contains(c)));
        }

    }
}
