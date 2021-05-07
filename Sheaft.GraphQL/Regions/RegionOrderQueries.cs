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

namespace Sheaft.GraphQL.Regions
{
    [ExtendObjectType(Name = "Query")]
    public class RegionOrderQueries : SheaftQuery
    {
        public RegionOrderQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("region")]
        [GraphQLType(typeof(RegionType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Region> Get([ID]Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Regions.Where(c => c.Id == id);
        }
        
        [GraphQLName("regions")]
        [GraphQLType(typeof(ListType<RegionType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Region> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Regions;
        }
    }
}