using System;
using System.Linq;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.Agreements
{
    [ExtendObjectType(Name = "Query")]
    public class AgreementQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;
        
        public AgreementQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<RoleOptions> roleOptions)
            :base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("agreement")]
        [GraphQLType(typeof(AgreementType))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UseSingleOrDefault]
        public IQueryable<Agreement> GetAgreement([ID("input")]Guid id, [ScopedService] AppDbContext context)
        {
            SetLogTransaction(id);
            
            if (CurrentUser.IsInRole(_roleOptions.Store.Value))
            {
                return context.Agreements
                    .Where(c => c.Id == id && c.StoreId == CurrentUser.Id);
            }
            
            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return context.Agreements
                    .Where(c => c.Id == id && c.ProducerId == CurrentUser.Id);
            }
        
            return context.Agreements;
        }
        
        [GraphQLName("agreements")]
        [GraphQLType(typeof(ListType<AgreementType>))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Agreement> GetAgreements([ScopedService] AppDbContext context)
        {
            SetLogTransaction();
            if (CurrentUser.IsInRole(_roleOptions.Store.Value))
            {
                return context.Agreements
                    .Where(c => c.StoreId == CurrentUser.Id && c.Status != AgreementStatus.Cancelled &&
                                c.Status != AgreementStatus.Refused);
            }
            
            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return context.Agreements
                    .Where(c => c.ProducerId == CurrentUser.Id && c.Status != AgreementStatus.Cancelled &&
                                c.Status != AgreementStatus.Refused);
            }
        
            return context.Agreements;
        }
    }
}