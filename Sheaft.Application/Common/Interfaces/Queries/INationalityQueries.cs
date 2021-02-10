using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface INationalityQueries
    {
        IQueryable<NationalityDto> GetNationalities(RequestUser currentUser);
        IQueryable<NationalityDto> GetNationality(Guid id, RequestUser currentUser);
    }
}