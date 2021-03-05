using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Withholding.Queries
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