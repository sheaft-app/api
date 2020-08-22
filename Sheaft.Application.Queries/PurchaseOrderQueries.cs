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
                            .Get(c => c.Id == id && c.Vendor.Id == currentUser.CompanyId)
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
                            .Get(c => c.Vendor.Id == currentUser.CompanyId)
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

        private static IQueryable<PurchaseOrderDto> GetOrdersAsDto(IQueryable<PurchaseOrder> query)
        {
            return query
                .Select(po =>
                    new PurchaseOrderDto
                    {
                        Comment = po.Comment,
                        CreatedOn = po.CreatedOn,
                        //ExpectedDelivery = new ExpectedDeliveryDto
                        //{
                        //    Day = po.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek,
                        //    DeliveredOn = po.ExpectedDelivery.DeliveredOn,                           
                        //    DeliveryStartedOn = po.ExpectedDelivery.DeliveryStartedOn,
                        //    ExpectedDeliveryDate = po.ExpectedDelivery.ExpectedDeliveryDate,
                        //    From = po.ExpectedDelivery.From,
                        //    To = po.ExpectedDelivery.To,
                        //    Address = new AddressDto
                        //    {
                        //        City = po.ExpectedDelivery.Address.City,
                        //        Latitude = po.ExpectedDelivery.Address.Latitude,
                        //        Line1 = po.ExpectedDelivery.Address.Line1,
                        //        Line2 = po.ExpectedDelivery.Address.Line2,
                        //        Longitude = po.ExpectedDelivery.Address.Longitude,
                        //        Zipcode = po.ExpectedDelivery.Address.Zipcode
                        //    },
                        //    Kind = po.ExpectedDelivery.Kind
                        //},
                        //Id = po.Id,
                        //LinesCount = po.LinesCount,
                        //UpdatedOn = po.UpdatedOn,
                        //Products = po.Products.Select(poq => new PurchaseOrderProductQuantityDto
                        //{
                        //    Id = poq.Id,
                        //    Name = poq.Name,
                        //    PackagingName = poq.PackagingName,
                        //    PackagingVat = poq.PackagingVat,
                        //    PackagingVatPrice = poq.PackagingVatPrice,
                        //    PackagingOnSalePrice = poq.PackagingOnSalePrice,
                        //    PackagingWholeSalePrice = poq.PackagingWholeSalePrice,
                        //    Quantity = poq.Quantity,
                        //    Reference = poq.Reference,
                        //    Vat = poq.Vat,
                        //    TotalOnSalePrice = poq.TotalOnSalePrice,
                        //    TotalVatPrice = poq.TotalVatPrice,
                        //    TotalWeight = poq.TotalWeight,
                        //    TotalWholeSalePrice = poq.TotalWholeSalePrice,
                        //    UnitOnSalePrice = poq.UnitOnSalePrice,
                        //    UnitVatPrice = poq.UnitVatPrice,
                        //    UnitWeight = poq.UnitWeight,
                        //    UnitWholeSalePrice = poq.UnitWholeSalePrice
                        //}),
                        //Reason = po.Reason,
                        //TotalWholeSalePrice = po.TotalWholeSalePrice,
                        //TotalWeight = po.TotalWeight,
                        //TotalVatPrice = po.TotalVatPrice,
                        //ProductsCount = po.ProductsCount,
                        //Reference = po.Reference,
                        //RemovedOn = po.RemovedOn,
                        //Status = po.Status,
                        //TotalOnSalePrice = po.TotalOnSalePrice,
                        //Sender = new UserProfileDto
                        //{
                        //    Id = po.Sender.Id,
                        //    Kind = po.Sender.Kind,
                        //    Email = po.Sender.Email,
                        //    Phone = po.Sender.Phone,
                        //    Name = po.Sender.Name,
                        //    Picture = po.Sender.Picture
                        //},
                        //Vendor = new UserProfileDto
                        //{
                        //    Id = po.Vendor.Id,
                        //    Kind = po.Vendor.Kind,
                        //    Email = po.Vendor.Email,
                        //    Phone = po.Vendor.Phone,
                        //    Name = po.Vendor.Name,
                        //    Picture = po.Vendor.Picture
                        //}
                    }
                );
        }
    }
}