using FluentValidation;
using Lipar.Core.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Lipar.Core.Application.Events;
using Lipar.Core.Contract.DependentyInjection;
using Lipar.Core.Contract.Utilities;
using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Common;

namespace Lipar.Tools.IoC.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddTransient<ServiceFactory>(p => p.GetService);
            services.AddTransient<IMediator, Mediator>();
            services.AddTransient<IEventPublisher, EventPublisher>();
            services.AddTransient<IEventBus, RabbitMQEventBus>();
            services.AddValidatorsFromAssemblies(assemblies);
          
            services.AddWithTransientLifetime(assemblies, typeof(IRequestHandler<>), typeof(IRequestHandler<,>));
            services.AddWithTransientLifetime(assemblies, typeof(IEventHandler<>), typeof(IOutBoxEventRepository), typeof(IEntityChangesInterceptorRepository));
            services.AddWithTransientLifetime(assemblies, typeof(IPipelineBehavior<,>), typeof(IPipelineBehavior<>));
            services.AddWithTransientLifetime(assemblies, typeof(ICommandRepository<>), typeof(IQueryRepository), typeof(IUnitOfWork));
        }

        public static void AddUtilities(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddWithTransientLifetime(assemblies, typeof(IJson), typeof(IDateTime), typeof(IUserInfo));

            services.AddWithTransientLifetime(assemblies, typeof(ITransientLifetime));
            services.AddWithScopedLifetime(assemblies, typeof(IScopeLifetime));
            services.AddWithSingletonLifetime(assemblies, typeof(ISingletoneLifetime));
        }

        public static void AddWithTransientLifetime(this IServiceCollection services,
           IEnumerable<Assembly> assembliesForSearch,
           params Type[] assignableTo)
        {
            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(c => c.AssignableToAny(assignableTo))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }

        public static void AddWithScopedLifetime(this IServiceCollection services,
           IEnumerable<Assembly> assembliesForSearch,
           params Type[] assignableTo)
        {
            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(c => c.AssignableToAny(assignableTo))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        public static void AddWithSingletonLifetime(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch,
            params Type[] assignableTo)
        {
            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(c => c.AssignableToAny(assignableTo))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }
    }
}
