using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Country.Queries
{
    public class CountryQueries : ICountryQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public CountryQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<CountryDto> GetCountry(Guid id, RequestUser currentUser)
        {
            return _context.Countries
                    .Where(d => d.Id == id)
                    .ProjectTo<CountryDto>(_configurationProvider);
        }

        public IQueryable<CountryDto> GetCountries(RequestUser currentUser)
        {
            return _context.Countries
                    .ProjectTo<CountryDto>(_configurationProvider);
        }
    }
}