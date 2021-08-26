using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Pickings
{
    [ExtendObjectType(Name = "Query")]
    public class PickingQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;

        public PickingQueries(
            IOptionsSnapshot<RoleOptions> roleOptions,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("picking")]
        [GraphQLType(typeof(PickingType))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Picking> GetPicking([ID] Guid id,
            [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            
            return context.Pickings
                .Where(c => c.Id == id);
        }

        [GraphQLName("pickings")]
        [GraphQLType(typeof(ListType<PickingType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Picking> GetAll([ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction();

            return context.Pickings
                .Where(c => c.ProducerId == CurrentUser.Id);
        }
    }
}