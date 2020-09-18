using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using AutoMapper.QueryableExtensions;

namespace Sheaft.Application.Queries
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