using System;
using System.Linq;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.BusinessClosing.Queries
{
    public class BusinessClosingQueries : IBusinessClosingQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public BusinessClosingQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser)
        {
            return _context.Set<Domain.BusinessClosing>()
                .Get(c => c.Id == id)
                .ProjectTo<ClosingDto>(_configurationProvider);
        }

        public IQueryable<ClosingDto> GetClosings(RequestUser currentUser)
        {
            return _context.Set<Domain.BusinessClosing>()
                .Get()
                .ProjectTo<ClosingDto>(_configurationProvider);
        }
    }
}