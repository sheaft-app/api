using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IPreAuthorizationQueries
    {
        IQueryable<PreAuthorizationDto> GetPreAuthorization(Guid id, RequestUser currentUser);
        IQueryable<PreAuthorizationDto> GetPreAuthorizations(RequestUser currentUser);
    }
}