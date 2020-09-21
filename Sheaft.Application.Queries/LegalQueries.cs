using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Queries
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
                    .Get(c => c.Consumer.Id == currentUser.Id)
                    .ProjectTo<ConsumerLegalDto>(_configurationProvider);
        }

        public IQueryable<BusinessLegalDto> GetMyBusinessLegals(RequestUser currentUser)
        {
            return _context.Legals
                    .OfType<BusinessLegal>()
                    .Get(c => c.Business.Id == currentUser.Id)
                    .ProjectTo<BusinessLegalDto>(_configurationProvider);
        }

        public IQueryable<ConsumerLegalDto> GetConsumerLegals(Guid id, RequestUser currentUser)
        {
            return _context.Legals
                    .OfType<ConsumerLegal>()
                    .Get(c => c.Id == id && c.Consumer.Id == currentUser.Id)
                    .ProjectTo<ConsumerLegalDto>(_configurationProvider);
        }

        public IQueryable<BusinessLegalDto> GetBusinessLegals(Guid id, RequestUser currentUser)
        {
            return _context.Legals
                    .OfType<BusinessLegal>()
                    .Get(c => c.Id == id && c.Business.Id == currentUser.Id)
                    .ProjectTo<BusinessLegalDto>(_configurationProvider);
        }
    }
}