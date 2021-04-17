using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Business.Factories
{
    public class ProductsImporterFactory : IProductsImporterFactory
    {
        private readonly IAppDbContext _context;
        private readonly ImportersOptions _options;

        public ProductsImporterFactory(IAppDbContext context, IOptions<ImportersOptions> options)
        {
            _context = context;
            _options = options.Value;
        }

        public IProductsFileImporter GetImporter(RequestUser requestUser, string typename)
        {
            return InstanciateImporter(requestUser, typename);
        }
        
        public async Task<IProductsFileImporter> GetImporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<User>(requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.ProductsImporter);
            
            return InstanciateImporter(requestUser, setting?.Value ?? _options.ProductsImporter);
        }

        private static IProductsFileImporter InstanciateImporter(RequestUser requestUser, string typename)
        {
            var type = Type.GetType(typename);
            if (type == null)
                throw new ArgumentException($"Invalid typename configured for user: {requestUser.Id} product importer.");

            if (!(Activator.CreateInstance(type) is IProductsFileImporter importer))
                throw new ArgumentException($"Invalid type used {type.FullName} for product importer.");

            return importer;
        }
    }
}