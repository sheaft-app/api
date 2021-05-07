using System.Linq;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Users
{
    [ExtendObjectType(Name = "Query")]
    public class ProfileQueries : SheaftQuery
    {
        public ProfileQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("myProducerProfile")]
        [GraphQLType(typeof(ProducerProfileType))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Producer> GetProducerProfile([ScopedService] QueryDbContext context)
        {
            SetLogTransaction(CurrentUser.Id);
            
            if (!CurrentUser.IsAuthenticated())
                return null;
            
            return context.Producers
                .Where(c => c.Id == CurrentUser.Id);
        }

        [GraphQLName("myStoreProfile")]
        [GraphQLType(typeof(StoreProfileType))]
        [Authorize(Policy = Policies.STORE)]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Store> GetStoreProfile([ScopedService] QueryDbContext context)
        {
            SetLogTransaction(CurrentUser.Id);
            
            if (!CurrentUser.IsAuthenticated())
                return null;

            return context.Stores
                .Where(c => c.Id == CurrentUser.Id);
        }

        [GraphQLName("myConsumerProfile")]
        [GraphQLType(typeof(ConsumerProfileType))]
        [Authorize(Policy = Policies.CONSUMER)]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Consumer> GetConsumerProfile([ScopedService] QueryDbContext context)
        {
            SetLogTransaction(CurrentUser.Id);
            
            if (!CurrentUser.IsAuthenticated())
                return null;

            return context.Consumers
                .Where(c => c.Id == CurrentUser.Id);
        }
    }
}