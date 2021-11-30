using System.Collections.Generic;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sheaft.Application.Behaviours;
using Sheaft.Application.Configurations;
using Sheaft.Application.Mediator;

namespace Sheaft.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            
            services.Configure<FreshdeskConfiguration>(configuration.GetSection(FreshdeskConfiguration.SETTING));
            services.Configure<PortalConfiguration>(configuration.GetSection(PortalConfiguration.SETTING));
            services.Configure<SireneApiConfiguration>(configuration.GetSection(SireneApiConfiguration.SETTING));
            services.Configure<RoutineConfiguration>(configuration.GetSection(RoutineConfiguration.SETTING));
            services.Configure<RoutineConfiguration>(configuration.GetSection(CorsConfiguration.SETTING));
            services.Configure<RoutineConfiguration>(configuration.GetSection(AppDatabaseConfiguration.SETTING));
            services.Configure<RoutineConfiguration>(configuration.GetSection(JobsDatabaseConfiguration.SETTING));
            services.Configure<RoutineConfiguration>(configuration.GetSection(StorageConfiguration.SETTING));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            
            services.AddMediatR(new List<Assembly>() { typeof(Command).Assembly }.ToArray());
        }
    }
}