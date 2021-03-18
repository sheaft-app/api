using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IWithholdingQueries
    {
        IQueryable<WithholdingDto> GetWithholding(Guid id, RequestUser currentUser);
        IQueryable<WithholdingDto> GetWithholdings(RequestUser currentUser);
    }
}