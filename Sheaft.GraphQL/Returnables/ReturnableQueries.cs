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

namespace Sheaft.GraphQL.Returnables
{
    [ExtendObjectType(Name = "Query")]
    public class ReturnableQueries : SheaftQuery
    {
        public ReturnableQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("returnable")]
        [GraphQLType(typeof(ReturnableType))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UseSingleOrDefault]
        public IQueryable<Returnable> Get([ID] Guid id, [ScopedService] AppDbContext context)
        {
            SetLogTransaction(id);
            return context.Returnables.Where(c => c.Id == id);
        }
        
        [GraphQLName("returnables")]
        [GraphQLType(typeof(ListType<ReturnableType>))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Returnable> GetAll([ScopedService] AppDbContext context)
        {
            SetLogTransaction();
            return context.Returnables.Where(c => c.ProducerId == CurrentUser.Id);
        }
    }
}