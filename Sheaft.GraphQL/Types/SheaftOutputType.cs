using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Extensions;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public abstract class SheaftOutputType<T> : ObjectType<T>
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            base.Configure(descriptor);
            descriptor.BindFieldsExplicitly();
        }
    }
}
