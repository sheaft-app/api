using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Interop;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class PackagingQueries : IPackagingQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public PackagingQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<PackagingDto> GetPackaging(Guid id, IRequestUser currentUser)
        {
            try
            {
                return _context.Packagings
                        .Get(c => c.Id == id && c.Producer.Id == currentUser.CompanyId)
                        .ProjectTo<PackagingDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<PackagingDto>().AsQueryable();
            }
        }

        public IQueryable<PackagingDto> GetPackagings(IRequestUser currentUser)
        {
            try
            {
                return _context.Packagings
                        .Get(c => c.Producer.Id == currentUser.CompanyId)
                        .ProjectTo<PackagingDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<PackagingDto>().AsQueryable();
            }
        }
        private static IQueryable<PackagingDto> GetAsDto(IQueryable<Packaging> query)
        {
            return query
                .Select(c => new PackagingDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedOn = c.CreatedOn,
                    Description = c.Description,
                    OnSalePrice = c.OnSalePrice,
                    Vat = c.Vat,
                    VatPrice = c.VatPrice,
                    WholeSalePrice = c.WholeSalePrice
                });
        }
    }
}