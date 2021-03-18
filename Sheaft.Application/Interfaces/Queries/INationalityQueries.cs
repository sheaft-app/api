using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface INationalityQueries
    {
        IQueryable<NationalityDto> GetNationalities(RequestUser currentUser);
        IQueryable<NationalityDto> GetNationality(Guid id, RequestUser currentUser);
    }
}