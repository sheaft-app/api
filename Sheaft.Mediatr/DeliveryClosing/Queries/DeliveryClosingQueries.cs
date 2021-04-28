using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.DeliveryClosing.Queries
{
    public class DeliveryClosingQueries : IDeliveryClosingQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly RoleOptions _roleOptions;

        public DeliveryClosingQueries(
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            AutoMapper.IConfigurationProvider configurationProvider)
        {
            _roleOptions = roleOptions.Value;
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.DeliveryMode>()
                    .Where(b => b.ProducerId == currentUser.Id && b.Closings.Any(c => c.Id == id))
                    .Select(b => b.Closings.SingleOrDefault(c => c.Id == id))
                    .ProjectTo<ClosingDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.DeliveryClosing>()
                .Where(c => c.Id == id)
                .ProjectTo<ClosingDto>(_configurationProvider);
        }

        public IQueryable<ClosingDto> GetClosings(Guid? deliveryId, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.DeliveryMode>()
                    .Where(b => b.ProducerId != currentUser.Id || !deliveryId.HasValue || b.Id == deliveryId.Value)
                    .SelectMany(b => b.Closings.Where(c => c.ClosedTo > DateTimeOffset.UtcNow))
                    .ProjectTo<ClosingDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.DeliveryMode>()
                .Where(b => !deliveryId.HasValue || b.Id == deliveryId.Value)
                .SelectMany(b => b.Closings)
                .ProjectTo<ClosingDto>(_configurationProvider);
        }
    }
}