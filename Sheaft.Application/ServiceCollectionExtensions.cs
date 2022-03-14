using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sheaft.Application;

public static class ServiceCollectionInitializers
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var assembly = typeof(ServiceCollectionInitializers).Assembly;
        
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes:true);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        return services;
    }
}