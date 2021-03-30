using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.ProductClosing.Queries
{
    public class ProductClosingQueries : IProductClosingQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly RoleOptions _roleOptions;

        public ProductClosingQueries(
            IAppDbContext context,
            IOptionsSnapshot<RoleOptions> roleOptions,
            AutoMapper.IConfigurationProvider configurationProvider)
        {
            _roleOptions = roleOptions.Value;
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.Product>()
                    .Get(b => b.Producer.Id == currentUser.Id && b.Closings.Any(c => c.Id == id))
                    .Select(b => b.Closings.SingleOrDefault(c => c.Id == id))
                    .ProjectTo<ClosingDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.ProductClosing>()
                .Get(c => c.Id == id)
                .ProjectTo<ClosingDto>(_configurationProvider);
        }

        public IQueryable<ClosingDto> GetClosings(RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.Product>()
                    .Get(b => b.Producer.Id == currentUser.Id)
                    .SelectMany(b => b.Closings)
                    .ProjectTo<ClosingDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.ProductClosing>()
                .Get()
                .ProjectTo<ClosingDto>(_configurationProvider);
        }
    }
}