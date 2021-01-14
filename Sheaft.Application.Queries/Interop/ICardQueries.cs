using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public interface ICardQueries
    {
        IQueryable<CardDto> GetCard(Guid id, RequestUser currentUser);
        IQueryable<CardDto> GetCards(RequestUser currentUser);
        IQueryable<CardDto> GetCardWithRegistrationId(Guid id, RequestUser currentUser);
    }
}