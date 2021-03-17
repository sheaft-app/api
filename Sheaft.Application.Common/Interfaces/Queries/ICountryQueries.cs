using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface ICountryQueries
    {
        IQueryable<CountryDto> GetCountries(RequestUser currentUser);
        IQueryable<CountryDto> GetCountry(Guid id, RequestUser currentUser);
    }
}