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

namespace Sheaft.GraphQL.Countries
{
    [ExtendObjectType(Name = "Query")]
    public class CountryQueries : SheaftQuery
    {
        public CountryQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("country")]
        [GraphQLType(typeof(CountryType))]
        [UseDbContext(typeof(AppDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Country> Get([ID] Guid id, [ScopedService] AppDbContext context)
        {
            SetLogTransaction(id);
            return context.Countries.Where(c => c.Id == id);
        }
        
        [GraphQLName("countries")]
        [GraphQLType(typeof(ListType<CountryType>))]
        [UseDbContext(typeof(AppDbContext))]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Country> GetAll([ScopedService] AppDbContext context)
        {
            SetLogTransaction();
            return context.Countries;
        }
    }
}