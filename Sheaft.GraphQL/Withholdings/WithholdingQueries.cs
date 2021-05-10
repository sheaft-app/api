using System;
using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Withholdings
{
    [ExtendObjectType(Name = "Query")]
    public class WithholdingQueries : SheaftQuery
    {
        public WithholdingQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("withholding")]
        [GraphQLType(typeof(WithholdingType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Withholding> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Withholdings
                .Where(d => d.Id == id && d.AuthorId == CurrentUser.Id);
        }
        
        [GraphQLName("withholdings")]
        [GraphQLType(typeof(ListType<WithholdingType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Withholding> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Withholdings
                .Where(d => d.AuthorId == CurrentUser.Id);
        }
    }
}