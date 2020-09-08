using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Core.Extensions;
using Sheaft.Infrastructure;
using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Application.Queries
{
    public class PurchaseOrderQueries : IPurchaseOrderQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly RoleOptions _roleOptions;

        public PurchaseOrderQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider, IOptionsSnapshot<RoleOptions> roleOptions)
        {
            _roleOptions = roleOptions.Value;
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<PurchaseOrderDto> MyPurchaseOrders(RequestUser currentUser)
        {
            try
            {
                return _context.PurchaseOrders
                        .Get(c => c.Sender.Id == currentUser.Id)
                        .ProjectTo<PurchaseOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<PurchaseOrderDto>().AsQueryable();
            }
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid id, RequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(_roleOptions.Producer.Value))
                    return _context.PurchaseOrders
                            .Get(c => c.Id == id && c.Vendor.Id == currentUser.Id)
                            .ProjectTo<PurchaseOrderDto>(_configurationProvider);

                return _context.PurchaseOrders
                        .Get(c => c.Id == id && c.Sender.Id == currentUser.Id)
                        .ProjectTo<PurchaseOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<PurchaseOrderDto>().AsQueryable();
            }
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrders(RequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(_roleOptions.Producer.Value))
                    return _context.PurchaseOrders
                            .Get(c => c.Vendor.Id == currentUser.Id)
                            .ProjectTo<PurchaseOrderDto>(_configurationProvider);

                return _context.PurchaseOrders
                        .Get(c => c.Sender.Id == currentUser.Id)
                        .ProjectTo<PurchaseOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<PurchaseOrderDto>().AsQueryable();
            }
        }
    }
}