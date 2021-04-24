using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Sheaft.Web.Common
{
    public static class ServiceCollectionExtensions
    {
        public static void AddScopedDynamic<TInterface>(this IServiceCollection services, IEnumerable<Type> types)
        {
            foreach (var type in types)
                services.AddScoped(type);
            
            services.AddScoped<Func<string, TInterface>>(serviceProvider => processor =>
            {
                var type = types
                    .Where(x => x.FullName == processor)
                    .FirstOrDefault();

                if (null == type)
                    throw new KeyNotFoundException("No instance found for the given processor.");

                var service = (TInterface)serviceProvider.GetService(type);
                return service;
            });
        }
    }
}