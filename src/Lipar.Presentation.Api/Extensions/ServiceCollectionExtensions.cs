using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lipar.Tools.IoC.Extensions;

namespace Lipar.Presentation.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLiparServices(this IServiceCollection services,
           IConfiguration configuration,
           params string[] assemblyNames)
        {
            var assembies = GetAssemblies(assemblyNames);

            services.AddApplication(assembies);
            services.AddUtilities(assembies);

            return services;
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
