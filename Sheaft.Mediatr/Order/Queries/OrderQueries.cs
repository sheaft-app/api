using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Order.Queries
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
            if (currentUser.IsAuthenticated)
                return _context.Orders
                    .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                    .ProjectTo<OrderDto>(_configurationProvider);
            
            return _context.Orders
                .Get(c => c.Id == id && c.Status == OrderStatus.Created && c.User == null)
                .ProjectTo<OrderDto>(_configurationProvider);
        }

        public IQueryable<OrderDto> GetOrders(RequestUser currentUser)
        {
            return _context.Orders
                    .Get(c => c.Status == OrderStatus.Validated)
                    .ProjectTo<OrderDto>(_configurationProvider);
        }

        public IQueryable<OrderDto> GetCurrentOrder(RequestUser currentUser)
        {
            return _context.Orders
                .Get(c => c.User.Id == currentUser.Id && c.Status == OrderStatus.Created)
                .OrderBy(c => c.CreatedOn)
                .ProjectTo<OrderDto>(_configurationProvider);
        }
    }
}