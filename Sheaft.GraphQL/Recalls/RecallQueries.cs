using System;
using System.Linq;
using System.Threading;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Recalls
{
    [ExtendObjectType(Name = "Query")]
    public class RecallQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;

        public RecallQueries(
            IOptionsSnapshot<RoleOptions> roleOptions,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("recall")]
        [GraphQLType(typeof(RecallType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Recall> GetRecall([ID] Guid id,
            [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            
            return context.Recalls
                .Where(c => c.Id == id);
        }

        [GraphQLName("recalls")]
        [GraphQLType(typeof(ListType<RecallType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Recall> GetAll([ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction();
            
            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
                return context.Recalls
                    .Where(c => c.ProducerId == CurrentUser.Id);

            return context.Recalls.Where(r => r.Clients.Any(c => c.ClientId == CurrentUser.Id) && r.Status != RecallStatus.Waiting);
        }
    }
}