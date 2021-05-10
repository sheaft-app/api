using System;
using System.Linq;
using HotChocolate.DataLoader;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sheaft.GraphQL;
using Sheaft.GraphQL.Types.Outputs;

namespace Sheaft.Web.Api.Extensions
{
    public static class SchemaBuilderExtensions
    {
        public static IRequestExecutorBuilder RegisterGraphQlQueries(this IRequestExecutorBuilder builder)
        {
            var types = typeof(UserAddressType).Assembly.GetExportedTypes()
                .Where(t => !t.IsGenericType && t.IsSubclassOf(typeof(SheaftQuery)))
                .ToArray();

            foreach (var type in types)
                builder.AddTypeExtension(type);
            
            return builder;
        }
        
        public static IRequestExecutorBuilder RegisterGraphQlMutations(this IRequestExecutorBuilder builder)
        {
            var types = typeof(UserAddressType).Assembly.GetExportedTypes()
                .Where(t => !t.IsGenericType && t.IsSubclassOf(typeof(SheaftMutation)))
                .ToArray();

            foreach (var type in types)
                builder.AddTypeExtension(type);
            
            return builder;
        }
        
        public static IRequestExecutorBuilder RegisterGraphQlTypes(this IRequestExecutorBuilder builder)
        {
            var types = typeof(UserAddressType).Assembly.GetExportedTypes()
                .Where(t => !t.IsGenericType && t.IsSubclassOf(typeof(SheaftOutputType<>)))
                .ToArray();

            builder.AddTypes(types);
            return builder;
        }

        public static IRequestExecutorBuilder RegisterGraphQlDataLoaders(this IRequestExecutorBuilder builder)
        {
            var types = typeof(UserAddressType).Assembly.GetExportedTypes()
                .Where(t => !t.IsGenericType && t.IsSubclassOf(typeof(BatchDataLoader<,>)))
                .ToArray();

            foreach (var type in types)
                builder = builder.AddDataLoader(type);

            return builder;
        }

        private static IRequestExecutorBuilder AddDataLoader(
            this IRequestExecutorBuilder builder,
            Type type)
        {
            builder.Services.TryAddScoped(type);
            return builder;
        }
    }
}