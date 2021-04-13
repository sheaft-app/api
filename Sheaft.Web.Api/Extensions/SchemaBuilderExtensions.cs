using System.Linq;
using System.Reflection;
using HotChocolate;
using Sheaft.GraphQL.Types.Outputs;

namespace Sheaft.Web.Api.Extensions
{
    public static class SchemaBuilderExtensions
    {
        public static ISchemaBuilder RegisterGraphQlTypes(this ISchemaBuilder services)
        {
            var types = typeof(AddressType).Assembly.GetExportedTypes()
                .Where(t => 
                    !t.IsGenericType &&
                    (t.Namespace.Contains(nameof(GraphQL.Enums))
                    || t.Namespace.Contains(nameof(GraphQL.Types.Outputs))
                    || t.Namespace.Contains(nameof(GraphQL.Types.Inputs))))
                .ToArray();
            
                services.AddTypes(types);

            return services;
        }
    }
}
