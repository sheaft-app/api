using Sheaft.Core;
using Sheaft.Models.Dto;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ICountryQueries
    {
        IQueryable<CountryDto> GetCountries(RequestUser currentUser);
        IQueryable<CountryDto> GetCountry(Guid id, RequestUser currentUser);
    }
}