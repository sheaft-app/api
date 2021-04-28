using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Legal.Queries
{
    public class LegalQueries : ILegalQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public LegalQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<ConsumerLegalDto> GetMyConsumerLegals(RequestUser currentUser)
        {
            return _context.Legals
                    .OfType<ConsumerLegal>()
                    .Where(c => c.UserId == currentUser.Id)
                    .ProjectTo<ConsumerLegalDto>(_configurationProvider);
        }

        public IQueryable<BusinessLegalDto> GetMyBusinessLegals(RequestUser currentUser)
        {
            return _context.Legals
                    .OfType<BusinessLegal>()
                    .Where(c => c.UserId == currentUser.Id)
                    .ProjectTo<BusinessLegalDto>(_configurationProvider);
        }

        public IQueryable<ConsumerLegalDto> GetConsumerLegals(Guid id, RequestUser currentUser)
        {
            return _context.Legals
                    .OfType<ConsumerLegal>()
                    .Where(c => c.Id == id && c.UserId == currentUser.Id)
                    .ProjectTo<ConsumerLegalDto>(_configurationProvider);
        }

        public IQueryable<BusinessLegalDto> GetBusinessLegals(Guid id, RequestUser currentUser)
        {
            return _context.Legals
                    .OfType<BusinessLegal>()
                    .Where(c => c.Id == id && c.UserId == currentUser.Id)
                    .ProjectTo<BusinessLegalDto>(_configurationProvider);
        }
    }
}