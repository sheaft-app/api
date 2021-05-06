using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.Business.Factories
{
    public class ProductsImporterFactory : IProductsImporterFactory
    {
        private readonly AppDbContext _context;
        private readonly Func<string, IProductsFileImporter> _resolver;
        private readonly ImportersOptions _options;

        public ProductsImporterFactory(IDbContextFactory<AppDbContext> context, IOptions<ImportersOptions> options, Func<string, IProductsFileImporter> resolver)
        {
            _context = context.CreateDbContext();
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