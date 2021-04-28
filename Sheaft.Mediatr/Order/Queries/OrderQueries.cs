﻿using System;
using System.Collections.Generic;
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
                    .Where(c => c.Id == id && c.UserId == currentUser.Id)
                    .ProjectTo<OrderDto>(_configurationProvider);

            return _context.Orders
                .Where(c => c.Id == id && c.Status == OrderStatus.Created && !c.UserId.HasValue)
                .ProjectTo<OrderDto>(_configurationProvider);
        }

        public IQueryable<OrderDto> GetOrders(RequestUser currentUser)
        {
            if (currentUser.IsAuthenticated)
                return _context.Orders
                    .Where(o => o.UserId == currentUser.Id)
                    .ProjectTo<OrderDto>(_configurationProvider);

            return new List<OrderDto>().AsQueryable();
        }

        public IQueryable<OrderDto> GetCurrentOrder(RequestUser currentUser)
        {
            if (currentUser.IsAuthenticated)
                return _context.Orders
                    .Where(c => c.UserId == currentUser.Id && c.Status == OrderStatus.Created)
                    .OrderBy(c => c.CreatedOn)
                    .ProjectTo<OrderDto>(_configurationProvider);

            return new List<OrderDto>().AsQueryable();
        }
    }
}