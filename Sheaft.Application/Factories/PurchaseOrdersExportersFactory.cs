using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces.Exporters;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Factories
{
    public class PurchaseOrdersExportersFactory : IPurchaseOrdersExportersFactory
    {
        private readonly IAppDbContext _context;
        private readonly Func<string, IPurchaseOrdersFileExporter> _resolver;
        private readonly ExportersOptions _options;

        public PurchaseOrdersExportersFactory(IAppDbContext context, IOptions<ExportersOptions> options, Func<string, IPurchaseOrdersFileExporter> resolver)
        {
            _context = context;
            _resolver = resolver;
            _options = options.Value;
        }

        public IPurchaseOrdersFileExporter GetExporter(RequestUser requestUser, string typename)
        {
            return InstanciateExporter(requestUser, typename);
        }
        
        public async Task<IPurchaseOrdersFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.PickingOrdersExporter);
            
            return InstanciateExporter(requestUser, setting?.Value ?? _options.PurchaseOrdersExporter);
        }

        private IPurchaseOrdersFileExporter InstanciateExporter(RequestUser requestUser, string typename)
        {
            return _resolver.Invoke(typename);
        }
    }
}