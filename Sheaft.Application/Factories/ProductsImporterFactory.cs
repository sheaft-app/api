﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Importers;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Factories
{
    public class ProductsImporterFactory : IProductsImporterFactory
    {
        private readonly IAppDbContext _context;
        private readonly Func<string, IProductsFileImporter> _resolver;
        private readonly ImportersOptions _options;

        public ProductsImporterFactory(IAppDbContext context, IOptions<ImportersOptions> options, Func<string, IProductsFileImporter> resolver)
        {
            _context = context;
            _resolver = resolver;
            _options = options.Value;
        }

        public IProductsFileImporter GetImporter(RequestUser requestUser, string typename)
        {
            return InstanciateImporter(requestUser, typename);
        }
        
        public async Task<IProductsFileImporter> GetImporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.ProductsImporter);
            
            return InstanciateImporter(requestUser, setting?.Value ?? _options.ProductsImporter);
        }

        private IProductsFileImporter InstanciateImporter(RequestUser requestUser, string typename)
        {
            return _resolver.Invoke(typename);
        }
    }
}