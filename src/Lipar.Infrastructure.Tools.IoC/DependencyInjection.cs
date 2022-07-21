using FluentValidation;
using Lipar.Core.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using Lipar.Core.Application.Events;
using Lipar.Core.Contract.DependentyInjection;
using Lipar.Core.Contract.Services;
using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Common;
using Lipar.Core.Application.Services;
using Lipar.Infrastructure.Tools.Utilities.Configurations;

namespace Lipar.Infrastructure.Tools.IoC;

public static partial class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services,
    IEnumerable<Assembly> assemblies,
    LiparOptions liparOptions)
    {
        services.AddTransient<ServiceFactory>(p => p.GetService);
        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IEventPublisher, EventPublisher>();
        services.AddTransient<IEmailService, EmailService>();

        services.AddWithTransientLifetime(assemblies, liparOptions.MessageBus.TypeName, typeof(IEventBus));
        services.AddWithTransientLifetime(assemblies, liparOptions.Translation.TypeName, typeof(ITranslator));
        services.AddWithTransientLifetime(assemblies, liparOptions.OutBoxEvent.TypeName, typeof(IOutBoxEventRepository));
        services.AddWithTransientLifetime(assemblies, liparOptions.InBoxEvent.TypeName, typeof(IInBoxEventRepository));
        services.AddWithTransientLifetime(assemblies, liparOptions.EntityChangesInterception.TypeName, typeof(IEntityChangesInterceptorRepository));

        services.AddValidatorsFromAssemblies(assemblies);
        services.AddWithTransientLifetime(assemblies, typeof(IRequestHandler<>), typeof(IRequestHandler<,>));
        services.AddWithTransientLifetime(assemblies, typeof(IEventHandler<>));
        services.AddWithTransientLifetime(assemblies, typeof(IPipelineBehavior<,>), typeof(IPipelineBehavior<>));
        services.AddWithTransientLifetime(assemblies, typeof(ICommandRepository<>), typeof(IQueryRepository));

        services.AddWithTransientLifetime(assemblies, typeof(IJsonService), typeof(IDateTimeService), typeof(IUserInfoService));
        services.AddWithTransientLifetime(assemblies, typeof(ITransientLifetime));
        services.AddWithScopedLifetime(assemblies, typeof(IScopeLifetime));
        services.AddWithSingletonLifetime(assemblies, typeof(ISingletoneLifetime));
    }
}



