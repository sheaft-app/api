using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.PurchaseOrder.Queries
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
            return _context.PurchaseOrders
                    .Get(c => c.Sender.Id == currentUser.Id)
                    .ProjectTo<PurchaseOrderDto>(_configurationProvider);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid id, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.PurchaseOrders
                            .Get(c => c.Id == id && c.Vendor.Id == currentUser.Id)
                            .ProjectTo<PurchaseOrderDto>(_configurationProvider);
            }

            return _context.PurchaseOrders
                        .Get(c => c.Id == id && c.Sender.Id == currentUser.Id)
                        .ProjectTo<PurchaseOrderDto>(_configurationProvider);
        }

        public IQueryable<PurchaseOrderDto> GetPurchaseOrders(RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return _context.PurchaseOrders
                            .Get(c => c.Vendor.Id == currentUser.Id)
                            .ProjectTo<PurchaseOrderDto>(_configurationProvider);
            }

            return _context.PurchaseOrders
                        .Get(c => c.Sender.Id == currentUser.Id)
                        .ProjectTo<PurchaseOrderDto>(_configurationProvider);
        }
    }
}