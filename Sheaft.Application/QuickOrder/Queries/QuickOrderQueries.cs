using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.QuickOrder.Queries
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
                    .Get(c => c.User.Id == userId && c.IsDefault)
                    .ProjectTo<QuickOrderDto>(_configurationProvider);
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, RequestUser currentUser)
        {
            return _context.QuickOrders
                    .Get(c => c.Id == quickOrderId && c.User.Id == currentUser.Id)
                    .ProjectTo<QuickOrderDto>(_configurationProvider);
        }

        public IQueryable<QuickOrderDto> GetQuickOrders(RequestUser currentUser)
        {
            return _context.QuickOrders
                    .Get(c => c.User.Id == currentUser.Id)
                    .ProjectTo<QuickOrderDto>(_configurationProvider);
        }
    }
}