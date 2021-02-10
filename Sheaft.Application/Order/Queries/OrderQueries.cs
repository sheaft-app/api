using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Order.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public OrderQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<OrderDto> GetOrder(Guid id, RequestUser currentUser)
        {
            return _context.Orders
                    .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                    .ProjectTo<OrderDto>(_configurationProvider);
        }

        public IQueryable<OrderDto> GetOrders(RequestUser currentUser)
        {
            return _context.Orders
                    .Get(c => c.User.Id == currentUser.Id)
                    .ProjectTo<OrderDto>(_configurationProvider);
        }
    }
}