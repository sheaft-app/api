using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.DeliveryClosing.Queries
{
    public class DeliveryClosingQueries : IDeliveryClosingQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public DeliveryClosingQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser)
        {
            return _context.Set<Domain.DeliveryClosing>()
                .Get(c => c.Id == id)
                .ProjectTo<ClosingDto>(_configurationProvider);
        }

        public IQueryable<ClosingDto> GetClosings(RequestUser currentUser)
        {
            return _context.Set<Domain.DeliveryClosing>()
                .Get()
                .ProjectTo<ClosingDto>(_configurationProvider);
        }
    }
}