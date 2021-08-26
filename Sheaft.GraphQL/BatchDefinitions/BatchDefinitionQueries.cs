using System;
using System.Linq;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.BatchDefinitions
{
    [ExtendObjectType(Name = "Query")]
    public class BatchDefinitionQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;
        
        public BatchDefinitionQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<RoleOptions> roleOptions)
            :base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("batchDefinition")]
        [GraphQLType(typeof(BatchDefinitionType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseSingleOrDefault]
        public IQueryable<BatchDefinition> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.BatchDefinitions
                .Where(c => c.Id == id);
        }
        
        [GraphQLName("batchDefinitions")]
        [GraphQLType(typeof(ListType<BatchDefinitionType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<BatchDefinition> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.BatchDefinitions
                .Where(c => c.ProducerId == CurrentUser.Id);
        }
    }
}