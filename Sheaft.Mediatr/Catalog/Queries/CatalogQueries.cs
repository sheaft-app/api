using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.Catalog.Queries
{
    public class CatalogQueries : ICatalogQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly RoleOptions _roleOptions;

        public CatalogQueries(
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            AutoMapper.IConfigurationProvider configurationProvider)
        {
            _roleOptions = roleOptions.Value;
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<CatalogDto> GetCatalog(Guid id, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.Catalog>()
                    .Where(b => b.Producer.Id == currentUser.Id && b.Id == id)
                    .ProjectTo<CatalogDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.Catalog>()
                .Where(c => c.Id == id)
                .ProjectTo<CatalogDto>(_configurationProvider);
        }

        public IQueryable<CatalogProductDto> GetCatalogProducts(Guid id, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.CatalogProduct>()
                    .Where(c => c.Catalog.Id == id && c.Catalog.Producer.Id == currentUser.Id)
                    .ProjectTo<CatalogProductDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.CatalogProduct>()
                .Where(c => c.Catalog.Id == id)
                .ProjectTo<CatalogProductDto>(_configurationProvider);
        }

        public IQueryable<CatalogDto> GetCatalogs(RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.Catalog>()
                    .Where(b => b.Producer.Id == currentUser.Id)
                    .ProjectTo<CatalogDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.Catalog>()
                .ProjectTo<CatalogDto>(_configurationProvider);
        }
    }
}