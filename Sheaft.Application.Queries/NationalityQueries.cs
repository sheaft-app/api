using System;
using System.Linq;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using AutoMapper.QueryableExtensions;

namespace Sheaft.Application.Queries
{
    public class NationalityQueries : INationalityQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public NationalityQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<NationalityDto> GetNationality(Guid id, RequestUser currentUser)
        {
            return _context.Nationalities
                    .Where(d => d.Id == id)
                    .ProjectTo<NationalityDto>(_configurationProvider);
        }

        public IQueryable<NationalityDto> GetNationalities(RequestUser currentUser)
        {
            return _context.Nationalities
                    .ProjectTo<NationalityDto>(_configurationProvider);
        }
    }
}