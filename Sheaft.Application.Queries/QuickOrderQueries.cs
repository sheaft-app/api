using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Interop.Enums;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
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
            try
            {
                return _context.QuickOrders
                        .Get(c => c.User.Id == userId && c.IsDefault)
                        .ProjectTo<QuickOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<QuickOrderDto>().AsQueryable();
            }
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, RequestUser currentUser)
        {
            try
            {
                return _context.QuickOrders
                        .Get(c => c.Id == quickOrderId && c.User.Id == currentUser.Id)
                        .ProjectTo<QuickOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<QuickOrderDto>().AsQueryable();
            }
        }

        public IQueryable<QuickOrderDto> GetQuickOrders(RequestUser currentUser)
        {
            try
            {
                return _context.QuickOrders
                        .Get(c => c.User.Id == currentUser.Id)
                        .ProjectTo<QuickOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<QuickOrderDto>().AsQueryable();
            }
        }
    }
}