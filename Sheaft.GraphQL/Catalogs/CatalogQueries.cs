using System;
using System.Linq;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Catalogs
{
    [ExtendObjectType(Name = "Query")]
    public class CatalogQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;
        
        public CatalogQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<RoleOptions> roleOptions)
            :base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("catalog")]
        [GraphQLType(typeof(CatalogType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseSingleOrDefault]
        public IQueryable<Catalog> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Set<Domain.Catalog>()
                .Where(c => c.Id == id);
        }
        
        [GraphQLName("catalogs")]
        [GraphQLType(typeof(ListType<CatalogType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Catalog> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Set<Domain.Catalog>()
                .Where(c => c.ProducerId == CurrentUser.Id);
        }
    }
}