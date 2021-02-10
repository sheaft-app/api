using Sheaft.Core;
using Sheaft.Application.Models;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface INationalityQueries
    {
        IQueryable<NationalityDto> GetNationalities(RequestUser currentUser);
        IQueryable<NationalityDto> GetNationality(Guid id, RequestUser currentUser);
    }
}