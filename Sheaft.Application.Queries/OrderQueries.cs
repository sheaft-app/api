using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
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
            try
            {
                return _context.Orders
                        .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                        .ProjectTo<OrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<OrderDto>().AsQueryable();
            }
        }

        public IQueryable<OrderDto> GetOrders(RequestUser currentUser)
        {
            try
            {
                return _context.Orders
                        .Get(c => c.User.Id == currentUser.Id)
                        .ProjectTo<OrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<OrderDto>().AsQueryable();
            }
        }
    }
}