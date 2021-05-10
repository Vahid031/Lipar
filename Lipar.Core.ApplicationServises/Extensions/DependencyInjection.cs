using Lipar.Core.ApplicationServises.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Lipar.Core.ApplicationServises.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IMediator, Mediator>();
            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.AddWithTransientLifetime(Assembly.GetExecutingAssembly(), typeof(IRequestHandler<>), typeof(IRequestHandler<,>));
            services.AddWithTransientLifetime(Assembly.GetExecutingAssembly(), typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddWithTransientLifetime(Assembly.GetExecutingAssembly(), typeof(IPipelineBehavior<>), typeof(ValidationBehavior<>));
        }

        public static IServiceCollection AddWithTransientLifetime(this IServiceCollection services,
            Assembly assembly,
            params Type[] assignableTo)
        {
            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableToAny(assignableTo))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            return services;
        }
    }
}

