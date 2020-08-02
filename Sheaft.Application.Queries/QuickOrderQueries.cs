using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Interop;
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

        public IQueryable<QuickOrderDto> GetUserDefaultQuickOrder(Guid userId, IRequestUser currentUser)
        {
            try
            {
                return _context.QuickOrders
                        .Get(c => c.User.Id == userId && c.IsDefault && c.User.Company.Id == currentUser.CompanyId)
                        .ProjectTo<QuickOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<QuickOrderDto>().AsQueryable();
            }
        }

        public IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, IRequestUser currentUser)
        {
            try
            {
                return _context.QuickOrders
                        .Get(c => c.Id == quickOrderId && c.User.Company.Id == currentUser.CompanyId)
                        .ProjectTo<QuickOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<QuickOrderDto>().AsQueryable();
            }
        }

        public IQueryable<QuickOrderDto> GetQuickOrders(IRequestUser currentUser)
        {
            try
            {
                return _context.QuickOrders
                        .Get(c => c.User.Company.Id == currentUser.CompanyId)
                        .ProjectTo<QuickOrderDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<QuickOrderDto>().AsQueryable();
            }
        }

        private static IQueryable<QuickOrderDto> GetAsDto(IQueryable<QuickOrder> query)
        {
            return query
                .Select(c => new QuickOrderDto
                {
                    CreatedOn = c.CreatedOn,
                    Id = c.Id,
                    UpdatedOn = c.UpdatedOn,
                    Products = c.Products.Select(p => new QuickOrderProductQuantityDto
                    {
                        Id = p.Product.Id,
                        Name = p.Product.Name,
                        Packaging = new PackagingDto
                        {
                            Id = p.Product.Packaging.Id,
                            Name = p.Product.Packaging.Name,
                            CreatedOn = p.Product.Packaging.CreatedOn,
                            Description = p.Product.Packaging.Description,
                            OnSalePrice = p.Product.Packaging.OnSalePrice,
                            Vat = p.Product.Packaging.Vat,
                            VatPrice = p.Product.Packaging.VatPrice,
                            WholeSalePrice = p.Product.Packaging.WholeSalePrice
                        },
                        Quantity = p.Quantity,
                        Reference = p.Product.Reference,
                        UnitOnSalePrice = p.Product.OnSalePrice,
                        UnitVatPrice = p.Product.VatPrice,
                        UnitWeight = p.Product.Weight,
                        UnitWholeSalePrice = p.Product.WholeSalePrice,
                        Vat = p.Product.Vat,
                        Producer = new CompanyProfileDto
                        {
                            Id = p.Product.Producer.Id,
                            Address = new AddressDto
                            {
                                City = p.Product.Producer.Address.City,
                                Latitude = p.Product.Producer.Address.Latitude,
                                Line1 = p.Product.Producer.Address.Line1,
                                Line2 = p.Product.Producer.Address.Line2,
                                Longitude = p.Product.Producer.Address.Longitude,
                                Zipcode = p.Product.Producer.Address.Zipcode
                            },
                            Email = p.Product.Producer.Email,
                            Name = p.Product.Producer.Name,
                            Phone = p.Product.Producer.Phone,
                            Picture = p.Product.Producer.Picture
                        }
                    }),
                    RemovedOn = c.RemovedOn,
                    Description = c.Description,
                    IsDefault = c.IsDefault,
                    Name = c.Name,
                    User = new UserProfileDto
                    {
                        Id = c.User.Id,
                        Email = c.User.Email,
                        Phone = c.User.Phone,
                        Kind = c.User.Company == null ? ProfileKind.Consumer : c.User.Company.Kind,
                        Name = c.User.FirstName + " " + c.User.LastName,
                        ShortName = c.User.FirstName + " " + c.User.LastName.Substring(0, 1) + ".",
                        Picture = c.User.Picture,
                        Initials = c.User.FirstName.Substring(0, 1) + c.User.LastName.Substring(0, 1)
                    }
                });
        }
    }
}