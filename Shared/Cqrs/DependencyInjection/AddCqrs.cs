using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs.Abstractions;
using Shared.Cqrs.Decorators;
using System.Reflection;
using Scrutor;

namespace Shared.Cqrs.DependencyInjection;

public static class AddCqrsExtension
{
    public static IServiceCollection AddCqrs(
        this IServiceCollection svc, params Assembly[] assemblies)
    {
        // 1. Register handlers
        svc.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime());

        // 2. Decorators (outermost first)
        svc.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandDecorator<>));
        svc.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingCommandDecorator<>));
        svc.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryDecorator<,>));

        // 3. MiniMediator
        svc.AddScoped<MiniMediator>();

        return svc;
    }
}
