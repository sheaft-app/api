using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Common.Extensions;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public abstract class SheaftOutputType<T> : ObjectType<T>
    {
        protected RequestUser GetRequestUser(IResolverContext context)
        {
            return GetRequestUser(context.ContextData);
        }
        protected RequestUser GetRequestUser(IDictionary<string, object> contextData)
        {
            var userDatas = (ClaimsPrincipal)contextData.FirstOrDefault(c => c.Key == nameof(ClaimsPrincipal)).Value;
            var httpDatas = (HttpContext)contextData.FirstOrDefault(c => c.Key == nameof(HttpContext)).Value;
            return userDatas.ToIdentityUser(httpDatas.TraceIdentifier);
        }
    }
}
