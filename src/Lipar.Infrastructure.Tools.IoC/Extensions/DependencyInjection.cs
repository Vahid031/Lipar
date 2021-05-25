﻿using FluentValidation;
using Lipar.Core.Application.Common;
using Lipar.Core.Domain.Data;
using Lipar.Infrastructure.Tools.Utilities.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Lipar.Infrastructure.Tools.Utilities.DependentyInjection;
using Lipar.Infrastructure.Tools.Utilities.Services;
using Lipar.Core.Application.Behaviors;

namespace Lipar.Tools.IoC.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddTransient<IMediator, Mediator>();
            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.AddValidatorsFromAssemblies(assemblies);
            services.AddWithTransientLifetime(assemblies, typeof(IRequestHandler<>), typeof(IRequestHandler<,>));
            services.AddWithTransientLifetime(assemblies, typeof(IPipelineBehavior<,>), typeof(IPipelineBehavior<>));
            services.AddWithTransientLifetime(assemblies, typeof(ICommandRepository<>), typeof(IQueryRepository), typeof(IUnitOfWork));
        }

        public static void AddUtilities(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddTransient<IJson, NewtonSoftSerializer>();
            services.AddTransient<IDateTime, UtcDateTime>();
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