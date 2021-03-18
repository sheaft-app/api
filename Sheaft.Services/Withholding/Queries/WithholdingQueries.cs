using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Services.Withholding.Queries
{
    public class WithholdingQueries : IWithholdingQueries
    {
        private readonly IAppDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        public WithholdingQueries(IAppDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<WithholdingDto> GetWithholding(Guid id, RequestUser currentUser)
        {
            return _context.Withholdings
                .Get(d => d.Id == id && d.Author.Id == currentUser.Id)
                    .ProjectTo<WithholdingDto>(_configurationProvider);
        }

        public IQueryable<WithholdingDto> GetWithholdings(RequestUser currentUser)
        {
            return _context.Withholdings
                    .Get(d => d.Author.Id == currentUser.Id)
                    .ProjectTo<WithholdingDto>(_configurationProvider);
        }
    }
}