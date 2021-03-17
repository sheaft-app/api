using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IWithholdingQueries
    {
        IQueryable<WithholdingDto> GetWithholding(Guid id, RequestUser currentUser);
        IQueryable<WithholdingDto> GetWithholdings(RequestUser currentUser);
    }
}