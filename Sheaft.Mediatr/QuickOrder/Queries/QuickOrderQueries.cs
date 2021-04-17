using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.QuickOrder.Queries
{
    public class QuickOrderQueries : IQuickOrderQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public QuickOrderQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<QuickOrderDto> GetUserDefaultQuickOrder(Guid userId, RequestUser currentUser)
        {
            return _context.QuickOrders
                .Where(c => c.User.Id == userId && c.IsDefault)
                .ProjectTo<QuickOrderDto>(_configurationProvider);
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, RequestUser currentUser)
        {
            return _context.QuickOrders
                .Where(c => c.Id == quickOrderId && c.User.Id == currentUser.Id)
                .ProjectTo<QuickOrderDto>(_configurationProvider);
        }

        public IQueryable<QuickOrderDto> GetQuickOrders(RequestUser currentUser)
        {
            return _context.QuickOrders
                .Where(c => c.User.Id == currentUser.Id)
                .ProjectTo<QuickOrderDto>(_configurationProvider);
        }
    }
}