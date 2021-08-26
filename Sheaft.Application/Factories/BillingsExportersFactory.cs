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
    public class BillingsExportersFactory : IBillingsExportersFactory
    {
        private readonly IAppDbContext _context;
        private readonly Func<string, IBillingsFileExporter> _resolver;
        private readonly ExportersOptions _options;

        public BillingsExportersFactory(IAppDbContext context, IOptions<ExportersOptions> options, Func<string, IBillingsFileExporter> resolver)
        {
            _context = context;
            _resolver = resolver;
            _options = options.Value;
        }

        public IBillingsFileExporter GetExporter(RequestUser requestUser, string typename)
        {
            return InstanciateExporter(requestUser, typename);
        }
        
        public async Task<IBillingsFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.DeliveriesExporter);
            
            return InstanciateExporter(requestUser, setting?.Value ?? _options.BillingsExporter);
        }

        private IBillingsFileExporter InstanciateExporter(RequestUser requestUser, string typename)
        {
            return _resolver.Invoke(typename);
        }
    }
}