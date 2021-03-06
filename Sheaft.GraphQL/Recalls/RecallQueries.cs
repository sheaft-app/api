using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

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
        [Authorize(Policy = Policies.REGISTERED)]
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

            return context.Recalls
                .Where(c => c.Clients.Any(b => b.ClientId == CurrentUser.Id));
        }
    }
}