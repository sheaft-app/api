using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Core.Extensions;
using Sheaft.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Sheaft.GraphQL.Types
{
    public abstract class SheaftOutputType<T> : ObjectType<T>
    {
        protected IRequestUser GetRequestUser(IResolverContext context)
        {
            return GetRequestUser(context.ContextData);
        }
        protected IRequestUser GetRequestUser(IDictionary<string, object> contextData)
        {
            var userDatas = (ClaimsPrincipal)contextData.FirstOrDefault(c => c.Key == nameof(ClaimsPrincipal)).Value;
            var httpDatas = (HttpContext)contextData.FirstOrDefault(c => c.Key == nameof(HttpContext)).Value;
            return userDatas.ToIdentityUser(httpDatas.TraceIdentifier);
        }
    }
    public abstract class SheaftInputType<T> : InputObjectType<T>
    {
        protected IRequestUser GetRequestUser(IResolverContext context)
        {
            return GetRequestUser(context.ContextData);
        }
        protected IRequestUser GetRequestUser(IDictionary<string, object> contextData)
        {
            var userDatas = (ClaimsPrincipal)contextData.FirstOrDefault(c => c.Key == nameof(ClaimsPrincipal)).Value;
            var httpDatas = (HttpContext)contextData.FirstOrDefault(c => c.Key == nameof(HttpContext)).Value;
            return userDatas.ToIdentityUser(httpDatas.TraceIdentifier);
        }
    }
}
