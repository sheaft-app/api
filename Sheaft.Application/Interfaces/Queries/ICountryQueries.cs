using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface ICountryQueries
    {
        IQueryable<CountryDto> GetCountries(RequestUser currentUser);
        IQueryable<CountryDto> GetCountry(Guid id, RequestUser currentUser);
    }
}