using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lipar.Infrastructure.Tools.IoC;

public static partial class DependencyInjection
{
    public static void AddWithTransientLifetime(this IServiceCollection services,
    IEnumerable<Assembly> assembliesForSearch,
    params Type[] assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
        .AddClasses(c => c.AssignableToAny(assignableTo))
        .AsImplementedInterfaces()
        .WithTransientLifetime());
    }

    public static void AddWithTransientLifetime(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch,
        string implementationTypeName,
        Type assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
        .AddClasses(c => c.Where(type => type.Name.ToLower().Equals(implementationTypeName.ToLower()) && assignableTo.IsAssignableFrom(type)))
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

    public static void AddWithScopedLifetime(this IServiceCollection services,
    IEnumerable<Assembly> assembliesForSearch,
    string implementationTypeName,
    Type assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
        .AddClasses(c => c.Where(type => type.Name.ToLower().Equals(implementationTypeName.ToLower()) && assignableTo.IsAssignableFrom(type)))
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

    public static void AddWithSingletonLifetime(this IServiceCollection services,
   IEnumerable<Assembly> assembliesForSearch,
   string implementationTypeName,
   Type assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
        .AddClasses(c => c.Where(type => type.Name.ToLower().Equals(implementationTypeName.ToLower()) && assignableTo.IsAssignableFrom(type)))
        .AsImplementedInterfaces()
        .WithSingletonLifetime());
    }

}


