using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public interface ICardRegistrationQueries
    {
        IQueryable<CardRegistrationDto> GetCardRegistration(Guid id, RequestUser currentUser);
        IQueryable<CardRegistrationDto> GetCardRegistrations(RequestUser currentUser);
    }
}