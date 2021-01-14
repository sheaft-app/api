using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public interface IPreAuthorizationQueries
    {
        IQueryable<PreAuthorizationDto> GetPreAuthorization(Guid id, RequestUser currentUser);
        IQueryable<PreAuthorizationDto> GetPreAuthorizations(RequestUser currentUser);
    }
}