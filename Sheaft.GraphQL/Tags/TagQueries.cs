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

namespace Sheaft.GraphQL.Tags
{
    [ExtendObjectType(Name = "Query")]
    public class TagQueries : SheaftQuery
    {
        public TagQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("tag")]
        [GraphQLType(typeof(TagType))]
        [UseDbContext(typeof(AppDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Tag> Get([ID] Guid id, [ScopedService] AppDbContext context)
        {
            SetLogTransaction(id);
            return context.Tags.Where(t => t.Id == id);
        }
        
        [GraphQLName("tags")]
        [GraphQLType(typeof(ListType<TagType>))]
        [UseDbContext(typeof(AppDbContext))]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Tag> GetAll([ScopedService] AppDbContext context)
        {
            SetLogTransaction();
            return context.Tags;
        }
    }
}