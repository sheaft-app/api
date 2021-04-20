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
using Sheaft.Options;

namespace Sheaft.Business.Factories
{
    public class TransactionsExportersFactory : ITransactionsExportersFactory
    {
        private readonly IAppDbContext _context;
        private readonly Func<string, ITransactionsFileExporter> _resolver;
        private readonly ExportersOptions _options;

        public TransactionsExportersFactory(IAppDbContext context, IOptions<ExportersOptions> options, Func<string, ITransactionsFileExporter> resolver)
        {
            _context = context;
            _resolver = resolver;
            _options = options.Value;
        }

        public ITransactionsFileExporter GetExporter(RequestUser requestUser, string typename)
        {
            return InstanciateExporter(requestUser, typename);
        }
        
        public async Task<ITransactionsFileExporter> GetExporterAsync(RequestUser requestUser, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == requestUser.Id, token);
            var setting = user.GetSetting(SettingKind.PickingOrdersExporter);
            
            return InstanciateExporter(requestUser, setting?.Value ?? _options.TransactionsExporter);
        }

        private ITransactionsFileExporter InstanciateExporter(RequestUser requestUser, string typename)
        {
            return _resolver.Invoke(typename);
        }
    }
}