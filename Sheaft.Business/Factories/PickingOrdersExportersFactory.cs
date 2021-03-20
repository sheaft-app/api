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

namespace Sheaft.Business
{
    public class PickingOrdersExportersFactory : IPickingOrdersExportersFactory
    {
        private readonly IAppDbContext _context;
        private readonly ExportersOptions _options;

        public PickingOrdersExportersFactory(IAppDbContext context, IOptions<ExportersOptions> options)
        {
            _context = context;
            _options = options.Value;
        }

        public IPickingOrdersFileExporter GetExporter(RequestUser requestUser, string typename)
        {
            return InstanciateExporter(requestUser, typename);
        }
        
        public async Task<IPickingOrdersFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<User>(requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.PickingOrdersExporter);
            
            return InstanciateExporter(requestUser, setting?.Value ?? _options.PickingOrdersExporter);
        }

        private static IPickingOrdersFileExporter InstanciateExporter(RequestUser requestUser, string typename)
        {
            var type = Type.GetType(typename);
            if (type == null)
                throw new ArgumentException($"Invalid typename configured for user: {requestUser.Id} picking orders exporter.");

            if (!(Activator.CreateInstance(type) is IPickingOrdersFileExporter importer))
                throw new ArgumentException($"Invalid type used {type.FullName} for picking orders exporter.");

            return importer;
        }
    }
}