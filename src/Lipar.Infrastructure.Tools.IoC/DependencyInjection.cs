using FluentValidation;
using Lipar.Core.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using Lipar.Core.Application.Events;
using Lipar.Core.Contract.DependentyInjection;
using Lipar.Core.Contract.Utilities;
using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Common;
using Lipar.Core.Application.Services;
using Lipar.Infrastructure.Events.RabbitMQ;

namespace Lipar.Infrastructure.Tools.IoC
{
    public static partial class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddTransient<ServiceFactory>(p => p.GetService);
            services.AddTransient<IMediator, Mediator>();
            services.AddTransient<IEventPublisher, EventPublisher>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEventBus, RabbitMQEventBus>();

            services.AddValidatorsFromAssemblies(assemblies);
            services.AddWithTransientLifetime(assemblies, typeof(IRequestHandler<>), typeof(IRequestHandler<,>));
            services.AddWithTransientLifetime(assemblies, typeof(IEventHandler<>), typeof(IOutBoxEventRepository), typeof(IInBoxEventRepository), typeof(IEntityChangesInterceptorRepository));
            services.AddWithTransientLifetime(assemblies, typeof(IPipelineBehavior<,>), typeof(IPipelineBehavior<>));
            services.AddWithTransientLifetime(assemblies, typeof(ICommandRepository<>), typeof(IQueryRepository), typeof(IUnitOfWork));

            services.AddWithTransientLifetime(assemblies, typeof(IJson), typeof(IDateTime), typeof(IUserInfo));
            services.AddWithTransientLifetime(assemblies, typeof(ITransientLifetime));
            services.AddWithScopedLifetime(assemblies, typeof(IScopeLifetime));
            services.AddWithSingletonLifetime(assemblies, typeof(ISingletoneLifetime));
        }
    }
      
}
