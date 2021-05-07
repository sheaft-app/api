using System;
using System.Linq;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Business
{
    [ExtendObjectType(Name = "Query")]
    public class BusinessQueries : SheaftQuery
    {
        public BusinessQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("businessClosing")]
        [GraphQLType(typeof(BusinessClosingType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UseSingleOrDefault]
        public IQueryable<BusinessClosing> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Set<BusinessClosing>()
                .Where(d => d.Id == id && d.BusinessId == CurrentUser.Id);
        }
        
        [GraphQLName("businessClosings")]
        [GraphQLType(typeof(ListType<BusinessClosingType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<BusinessClosing> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Set<BusinessClosing>()
                .Where(d => d.BusinessId == CurrentUser.Id);
        }
    }
}