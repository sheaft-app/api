using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface ICardQueries
    {
        IQueryable<CardDto> GetCard(Guid id, RequestUser currentUser);
        IQueryable<CardDto> GetCards(RequestUser currentUser);
    }
}