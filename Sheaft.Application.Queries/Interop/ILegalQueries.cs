using Sheaft.Core;
using Sheaft.Models.Dto;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ILegalQueries
    {
        IQueryable<T> GetLegal<T>(Guid id, RequestUser currentUser) where T : BaseLegalDto;
        IQueryable<T> GetLegals<T>(RequestUser currentUser) where T : BaseLegalDto;
    }
}