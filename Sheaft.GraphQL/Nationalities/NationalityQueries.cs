using System;
using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Nationalities
{
    [ExtendObjectType(Name = "Query")]
    public class NationalityQueries : SheaftQuery
    {
        public NationalityQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("nationality")]
        [GraphQLType(typeof(NationalityType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Nationality> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Nationalities.Where(n => n.Id == id);
        }
        
        [GraphQLName("nationalities")]
        [GraphQLType(typeof(ListType<NationalityType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Nationality> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Nationalities;
        }
    }
}